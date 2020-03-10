﻿using System.Collections.Generic;
using WebValidation.Model;
using WebValidation.Parameters;
using Xunit;

namespace UnitTests
{
    public class TestCommonTarget
    {
        [Fact]
        public void PathTest()
        {
            ValidationResult res;

            // empty path
            res = Validator.ValidatePath(string.Empty);
            Assert.True(res.Failed);

            // path must start with /
            res = Validator.ValidatePath("testpath");
            Assert.True(res.Failed);
        }

        [Fact]
        public void CommonBoundariesTest()
        {
            ValidationResult res;

            // verb must be GET POST PUT DELETE ...
            // path must start with /
            Request r = new Request
            {
                Verb = "badverb",
                Path = "badpath",
                Validation = null
            };
            res = Validator.Validate(r);
            Assert.True(res.Failed);

            Validation v = new Validation();

            // null is valid
            res = Validator.ValidateLength(null);
            Assert.False(res.Failed);

            // edge values
            // >= 0
            v.Length = -1;
            v.MinLength = -1;
            v.MaxLength = -1;

            // 200 - 599
            v.StatusCode = 10;

            // > 0
            v.MaxMilliseconds = 0;

            // ! isnullorempty
            v.ExactMatch = string.Empty;
            v.ContentType = string.Empty;

            // each element ! isnullempty
            v.Contains = new List<string> { string.Empty };

            res = Validator.Validate(v);
            Assert.True(res.Failed);
        }

        [Fact]
        public void PerfLogTest()
        {
            var p = new PerfLog
            {
                Date = new System.DateTime(2020, 1, 1),
                ValidationResults = "test"
            };

            // validate getters and setters
            Assert.Equal(new System.DateTime(2020, 1, 1), p.Date);
            Assert.Equal("test", p.ValidationResults);
        }

        [Fact]
        public void PerfTargetTest()
        {
            ValidationResult res = new ValidationResult();

            // category can't be blank
            PerfTarget t = new PerfTarget();
            res = Validator.Validate(t);
            Assert.True(res.Failed);

            // quartiles can't be null
            t.Category = "Tests";
            res = Validator.Validate(t);
            Assert.True(res.Failed);

            // valid
            t.Quartiles = new List<double> { 100, 200, 400 };
            res = Validator.Validate(t);
            Assert.False(res.Failed);

        }
    }
}
