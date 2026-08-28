using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        // NOTE: hardcoded demo credential for assessment purposes.
        // In production this would validate against a Users table with hashed passwords.
        private const string DemoUserName = "admin";
        private const string DemoPassword = "Admin@123";

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            if (dto.UserName != DemoUserName || dto.Password != DemoPassword)
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Credentials", new[] { "Invalid username or password." } }
            });

            return await IssueTokensAsync(dto.UserName);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken)
                ?? throw new NotFoundException(nameof(RefreshToken), refreshToken);

            if (!existingToken.IsActive)
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "RefreshToken", new[] { "Refresh token is expired or revoked." } }
            });

            existingToken.IsRevoked = true;
            _unitOfWork.RefreshTokens.Update(existingToken);

            var response = await IssueTokensAsync(existingToken.UserName);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken)
                ?? throw new NotFoundException(nameof(RefreshToken), refreshToken);

            existingToken.IsRevoked = true;
            _unitOfWork.RefreshTokens.Update(existingToken);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<AuthResponseDto> IssueTokensAsync(string userName)
        {
            var accessToken = _tokenService.GenerateAccessToken(userName);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var entity = new RefreshToken
            {
                Token = refreshToken,
                UserName = userName,
                ExpiresOn = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays)
            };

            await _unitOfWork.RefreshTokens.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresOn = entity.ExpiresOn
            };
        }
    }
}
