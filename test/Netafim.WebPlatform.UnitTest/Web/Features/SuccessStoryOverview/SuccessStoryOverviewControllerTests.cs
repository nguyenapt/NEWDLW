using System;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview.Repository;

namespace Netafim.WebPlatform.UnitTest.Web.Features.SuccessStoryOverview
{
    [TestClass]
    public class SuccessStoryOverviewControllerTests
    {
        public void TestInitialize()
        {
            
        }

        /*
         scenario 1: Boosted result still return match with pageSize

            input:
                boostedItems: 3
                boostedTotalMatching: 7 => TotalPage: 3
                pageSize: 3 
                currentPage: 0 | 1
                hasHashData: false
            output:
                skip: 0
                take: 0
        */

        [TestMethod]
        public void SuccessStoryOverviewControllerTests_Boosted_Result_Still_Match_Enough()
        {
            // Arrange
            var boostedItems = 3;
            var boostedTotalMatching = 7;
            var pageSize = 3;
            var currentPage0 = 0;

            // Act
            var resOfCurrentPage0 = SuccessStoryOverviewExtensions.CalculateTakeAndSkipItem(boostedItems, boostedTotalMatching, pageSize, currentPage0, false);

            // Assert
            Assert.AreEqual(new Tuple<int,int>(0, 0), resOfCurrentPage0);

            var currentPage1 = 1;

            // Act
            var resOfCurrentPage1 = SuccessStoryOverviewExtensions.CalculateTakeAndSkipItem(boostedItems, boostedTotalMatching, pageSize, currentPage1, false);

            // Assert
            Assert.AreEqual(new Tuple<int, int>(0, 0), resOfCurrentPage1);
        }

        /*
         scenario 2: Boosted result return not match with pageSize, 
                     Need take deficit = 2  pageSize - (boostedTotalMatching % pageSize)

            input:
                boostedItems: 1
                boostedTotalMatching: 7 => TotalPage: 3
                pageSize: 3 
                currentPage: 2
                hasHashData: false
            output:
                skip: 0
                take: 2
        */

        [TestMethod]
        public void SuccessStoryOverviewControllerTests_Boosted_Result_Not_Match_Enough()
        {
            // Arrange
            var boostedItems = 1;
            var boostedTotalMatching = 7;
            var pageSize = 3;
            var currentPage2 = 2;

            // Act
            var resOfCurrentPage0 = SuccessStoryOverviewExtensions.CalculateTakeAndSkipItem(boostedItems, boostedTotalMatching, pageSize, currentPage2, false);

            // Assert
            Assert.AreEqual(new Tuple<int, int>(0, 2), resOfCurrentPage0);
        }

        /*
         scenario 3: Calculate skip and take for query from the rest of items
                    
            input:
                boostedItems: 0
                boostedTotalMatching: 7 => TotalPage: 3
                pageSize: 3 
                currentPage: 3
                hasHashData: false
            output:
                skip: 2 
                take: 3
        */

        [TestMethod]
        public void SuccessStoryOverviewControllerTests_Go_Over_Boosted_Get_Item_From_Rest_Of_Items()
        {
            // Arrange
            var boostedItems = 0;
            var boostedTotalMatching = 7;
            var pageSize = 3;
            var currentPage3 = 3;

            // Act
            var resOfCurrentPage3 = SuccessStoryOverviewExtensions.CalculateTakeAndSkipItem(boostedItems, boostedTotalMatching, pageSize, currentPage3, false);

            // Assert
            Assert.AreEqual(new Tuple<int, int>(2, 3), resOfCurrentPage3);
        }
    }
}
