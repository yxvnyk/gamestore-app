using System;
using System.Linq;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gamestore.Domain.Tests.Extensions
{
    public class LoggerExtensionsTests
    {
        [Fact]
        public void LogRequestDetails_CallsLoggerWithCorrectMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            
            loggerMock.Setup(l => l.IsEnabled(LogLevel.Information)).Returns(true);

            var model = new RequestDetailsModel
            {
                RemoteIpAddress = "127.0.0.1",
                TargetUrl = "http://localhost/test",
                ResponseStatusCode = 200,
                RequestContent = "request",
                ResponseContent = "response",
                ElapsedTime = "100ms"
            };

            // Act
            loggerMock.Object.LogRequestDetails(model);

            var invocation = loggerMock.Invocations
                .FirstOrDefault(i => i.Method.Name == "Log" && i.Arguments.Count > 0 && i.Arguments[0] is LogLevel lvl && lvl == LogLevel.Information);

            // Assert
            Assert.NotNull(invocation);
            var state = invocation.Arguments[2];
            var formatted = state?.ToString() ?? string.Empty;
            Assert.Contains(model.RemoteIpAddress, formatted);
            Assert.Contains(model.TargetUrl, formatted);
            Assert.Contains(model.ResponseStatusCode.ToString(), formatted);
            Assert.Contains(model.RequestContent, formatted);
            Assert.Contains(model.ResponseContent, formatted);
            Assert.Contains(model.ElapsedTime, formatted);
        }

        [Fact]
        public void LogTrace_CallsLoggerWithCorrectClassName()
        {
            var loggerMock = new Mock<ILogger>();
            const string className = "TestClass";

            loggerMock.Setup(l => l.IsEnabled(LogLevel.Trace)).Returns(true);

            // Act
            loggerMock.Object.LogTrace(className);

            // Assert
            var invocation = loggerMock.Invocations
                .FirstOrDefault(i => i.Method.Name == "Log" && i.Arguments.Count > 0 && i.Arguments[0] is LogLevel lvl && lvl == LogLevel.Trace);

            Assert.NotNull(invocation);
            var state = invocation.Arguments[2];
            var formatted = state?.ToString() ?? string.Empty;
            Assert.Contains(className, formatted);
        }

        [Fact]
        public void LogException_WithInnerAndData_CallsLogger()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var inner = new Exception("inner");
            var ex = new Exception("outer", inner);
            ex.Data["key"] = "value";

            loggerMock.Setup(l => l.IsEnabled(LogLevel.Error)).Returns(true);

            // Act
            loggerMock.Object.LogException(ex);

            // Assert
            var logInvocation = loggerMock.Invocations
                .FirstOrDefault(i => i.Method.Name == "Log" && i.Arguments.Count > 0 && i.Arguments[0] is LogLevel lvl && lvl == LogLevel.Error);

            Assert.NotNull(logInvocation);

            var state = logInvocation.Arguments[2];
            var formatted = state?.ToString() ?? string.Empty;

            Assert.Contains("outer", formatted);
            Assert.Contains("inner", formatted);
            Assert.Contains("key", formatted);

            Assert.Same(ex, logInvocation.Arguments[3]);
        }
    }
}