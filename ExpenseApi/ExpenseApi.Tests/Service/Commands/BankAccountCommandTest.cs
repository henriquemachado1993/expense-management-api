using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;

using ExpenseApi.Service.Service;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;


namespace ExpenseApi.Tests.Service.Commands
{
    public class BankAccountCommandTest
    {
        private Mock<IBankAccountService> _service;

        [SetUp]
        public void Setup()
        {
            _service = new Mock<IBankAccountService>();
        }
    }
}
