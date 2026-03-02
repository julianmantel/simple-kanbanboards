using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleKanbanBoards.Business.Utils;
using Xunit;

namespace SimpleKanbanBoards.Tests.Unit.Utils
{
    public class AuthUtilTests
    {
        [Fact]
        public void CreatePasswordHash_ShouldReturnNonEmptyHashAndSalt()
        {
            AuthUtil.CreatePasswordHash("MyPassword1", out byte[] hash, out byte[] salt);

            hash.Should().NotBeNullOrEmpty();
            salt.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CreatePasswordHash_ShouldGenerateDifferentSaltsEachTime()
        {
            AuthUtil.CreatePasswordHash("SamePassword1", out _, out byte[] salt1);
            AuthUtil.CreatePasswordHash("SamePassword1", out _, out byte[] salt2);

            salt1.Should().NotEqual(salt2);
        }

        [Fact]
        public void CreatePasswordHash_SameSaltShouldProduceSameHash()
        {
            AuthUtil.CreatePasswordHash("Password1", out byte[] hash1, out byte[] salt1);

            using var hmac = new System.Security.Cryptography.HMACSHA512(salt1);
            var recomputed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Password1"));

            recomputed.Should().Equal(hash1);
        }

        [Fact]
        public void VerifyPasswordHash_WithCorrectPassword_ShouldReturnTrue()
        {
            AuthUtil.CreatePasswordHash("CorrectPass1", out byte[] hash, out byte[] salt);

            var isValid = AuthUtil.VerifyPasswordHash("CorrectPass1", hash, salt);

            isValid.Should().BeTrue();
        }

        [Fact]
        public void VerifyPasswordHash_WithWrongPassword_ShouldReturnFalse()
        {
            AuthUtil.CreatePasswordHash("CorrectPass1", out byte[] hash, out byte[] salt);

            var isValid = AuthUtil.VerifyPasswordHash("WrongPass1", hash, salt);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void VerifyPasswordHash_WithEmptyPassword_ShouldReturnFalse()
        {
            AuthUtil.CreatePasswordHash("CorrectPass1", out byte[] hash, out byte[] salt);

            var isValid = AuthUtil.VerifyPasswordHash("", hash, salt);

            isValid.Should().BeFalse();
        }
    }
}
