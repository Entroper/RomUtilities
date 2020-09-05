using System;
using RomUtilities;
using Xunit;

namespace RomUtilities.Tests
{
    public class BlobTests
    {
        [Fact]
        public void BlobsAreEqual()
        {
            var lhs = Blob.FromHex("1234567890");
            var rhs = Blob.FromHex("1234567890");
            
            Assert.Equal(lhs, rhs);
            Assert.True(lhs == rhs);
        }

        [Fact]
        public void BlobsAreNotEqual()
        {
            var lhs = Blob.FromHex("1234567890");
            var rhs = Blob.FromHex("9876543210");
            
            Assert.NotEqual(lhs, rhs);
            Assert.False(lhs == rhs);
        }

        [Fact]
        public void Concatenation()
        {
            var lhs = Blob.FromHex("1234567890");
            var rhs = Blob.FromHex("9876543210");
            var concatenated = Blob.FromHex("12345678909876543210");

            Assert.Equal(concatenated, lhs + rhs);
        }

        [Fact]
        public void SubBlobs()
        {
            var theBlob = Blob.FromHex("1234567890");
            var sub1 = Blob.FromHex("123456");
            var sub2 = Blob.FromHex("567890");

            Assert.Equal(sub1, theBlob.SubBlob(0, 3));
            Assert.Equal(sub2, theBlob.SubBlob(2));

            Assert.ThrowsAny<Exception>(() =>
            {
                theBlob.SubBlob(7);
            });
            Assert.ThrowsAny<Exception>(() =>
            {
                theBlob.SubBlob(4, 3);
            });
        }

        [Fact]
        public void IndexOf()
        {
            var theBlob = Blob.FromHex("1234567890");
            var findThis = Blob.FromHex("1234");
            var findThisToo = Blob.FromHex("5678");
            var findThisThree = Blob.FromHex("7890");
            var dontFindThis = Blob.FromHex("0000");
            var dontFindThisEither = Blob.FromHex("789012");
            var definitelyDontFindThis = Blob.FromHex("123456789012");

            Assert.Equal(0, theBlob.IndexOf(0x12));
            Assert.Equal(2, theBlob.IndexOf(0x56));
            Assert.Equal(4, theBlob.IndexOf(0x90));
            Assert.Equal(-1, theBlob.IndexOf(0x00));

            Assert.Equal(0, theBlob.IndexOf(findThis));
            Assert.Equal(2, theBlob.IndexOf(findThisToo));
            Assert.Equal(3, theBlob.IndexOf(findThisThree));
            Assert.Equal(-1, theBlob.IndexOf(dontFindThis));
            Assert.Equal(-1, theBlob.IndexOf(dontFindThisEither));
            Assert.Equal(-1, theBlob.IndexOf(definitelyDontFindThis));
        }

        [Fact]
        public void ReplaceInPlace()
        {
            var original1 = Blob.FromHex("12345678901234567890");
            var search1 = Blob.FromHex("1234");
            var replace1 = Blob.FromHex("4321");
            var expected1 = Blob.FromHex("43215678904321567890");

            var original2 = Blob.FromHex("12345678901234567890");
            var search2 = Blob.FromHex("7890");
            var replace2 = Blob.FromHex("4321");
            var expected2 = Blob.FromHex("12345643211234564321");

            var dontReplace = Blob.FromHex("654321");
            Assert.ThrowsAny<Exception>(() =>
            {
                original1.ReplaceInPlace(search1, dontReplace);
            });

            original1.ReplaceInPlace(search1, replace1);
            Assert.Equal(expected1, original1);

            original2.ReplaceInPlace(search2, replace2);
            Assert.Equal(expected2, original2);
        }

        [Fact]
        public void ReplaceOutOfPlace()
        {
            var original = Blob.FromHex("12345678901234567890");

            var search1 = Blob.FromHex("1234");
            var replace1 = Blob.FromHex("654321");
            var expected1 = Blob.FromHex("654321567890654321567890");

            var search2 = Blob.FromHex("7890");
            var replace2 = Blob.FromHex("65");
            var expected2 = Blob.FromHex("1234566512345665");

            var new1 = original.ReplaceOutOfPlace(search1, replace1);
            Assert.Equal(expected1, new1);

            var new2 = original.ReplaceOutOfPlace(search2, replace2);
            Assert.Equal(expected2, new2);
        }
    }
}
