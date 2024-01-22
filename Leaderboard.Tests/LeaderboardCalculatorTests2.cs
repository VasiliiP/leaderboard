using Leaderboard.Models;
using Leaderboard.Services;

namespace Leaderboard.Tests;

public class LeaderboardCalculatorTests2
{
    private readonly ILeaderboardCalculator leaderboardCalculator;

    public LeaderboardCalculatorTests2()
    {
        this.leaderboardCalculator = new LeaderboardCalculator2();
    }

    [Fact]
    public void NotEnoughScoreForFirstPlaces()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new User(userId: 1, score: 3),
            new User(userId: 2, score: 2),
            new User(userId: 3, score: 1)
        };

        var result = this.leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new UserWithPlace(userId: 1, place: 4),
            new UserWithPlace(userId: 2, place: 5),
            new UserWithPlace(userId: 3, place: 6)
        };

        Assert.True(CheckResult(result, expectedResult));
    }
    
    [Fact]
    public void AllPlacesAndLowScores()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new User(userId: 1, score: 100),
            new User(userId: 10, score: 120),
            new User(userId: 2, score: 3),
            new User(userId: 3, score: 2),
            new User(userId: 4, score: 1),
            new User(userId: 5, score: 99),
            new User(userId: 6, score: 98),
            new User(userId: 7, score: 97),
            new User(userId: 8, score: 96)

        };

        var result = this.leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new UserWithPlace(userId: 10, place: 1),
            new UserWithPlace(userId: 1, place: 2),
            new UserWithPlace(userId: 2, place: 7),
            new UserWithPlace(userId: 3, place: 8),
            new UserWithPlace(userId: 4, place: 9),
            new UserWithPlace(userId: 5, place: 3),
            new UserWithPlace(userId: 6, place: 4),
            new UserWithPlace(userId: 7, place: 5),
            new UserWithPlace(userId: 8, place: 6),
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    [Fact]
    public void OnlyFirstPlaceAndLowScores()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new User(userId: 1, score: 100),
            new User(userId: 2, score: 3),
            new User(userId: 3, score: 2),
            new User(userId: 4, score: 1)
        };

        var result = this.leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new UserWithPlace(userId: 1, place: 1),
            new UserWithPlace(userId: 2, place: 4),
            new UserWithPlace(userId: 3, place: 5),
            new UserWithPlace(userId: 4, place: 6)
        };

        Assert.True(CheckResult(result, expectedResult));
    }
    
    [Fact]
    public void TwoFirstPlacesAndLowScores()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new User(userId: 1, score: 100),
            new User(userId: 10, score: 120),
            new User(userId: 2, score: 3),
            new User(userId: 3, score: 2),
            new User(userId: 4, score: 1)
        };

        var result = this.leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new UserWithPlace(userId: 10, place: 1),
            new UserWithPlace(userId: 1, place: 2),
            new UserWithPlace(userId: 2, place: 4),
            new UserWithPlace(userId: 3, place: 5),
            new UserWithPlace(userId: 4, place: 6)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    [Fact]
    public void OnlySecondPlace()
    {
        var minScores = new LeaderboardMinScores(100, 50, 10);
        var usersWithScores = new List<User>
        {
            new User(userId: 1, score: 55)
        };

        var result = this.leaderboardCalculator.CalculatePlaces(usersWithScores, minScores);

        var expectedResult = new List<UserWithPlace>
        {
            new UserWithPlace(userId: 1, place: 2)
        };

        Assert.True(CheckResult(result, expectedResult));
    }

    private static bool CheckResult(IEnumerable<UserWithPlace> result, IEnumerable<UserWithPlace> expectedResult)
    {
        if (result == null)
        {
            return false;
        }

        List<UserWithPlace> resultList;
        try
        {
            resultList = result.ToList();
        }
        catch
        {
            return false;
        }

        var expectedResultList = expectedResult.ToList();
        if (resultList.Count != expectedResultList.Count)
        {
            return false;
        }

        foreach (var expectedResultUser in expectedResultList)
        {
            var resultUser = resultList.FirstOrDefault(e => e?.UserId == expectedResultUser.UserId);
            if (resultUser == null || resultUser.Place != expectedResultUser.Place)
            {
                return false;
            }
        }

        return true;
    }
}
