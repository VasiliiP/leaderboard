using Leaderboard.Models;

namespace Leaderboard.Services;

public class LeaderboardCalculator2 : ILeaderboardCalculator
{
    public IEnumerable<UserWithPlace> CalculatePlaces(IEnumerable<IUserWithScore> usersWithScores,
        LeaderboardMinScores leaderboardMinScores)
    {
        var sortedUsers = usersWithScores
            .OrderByDescending(x => x.Score)
            .ToList();
        
        var usersWithPlaces = new List<UserWithPlace>(sortedUsers.Count);
        var place = 1;
        foreach (var user in sortedUsers)
        {
            place = SetUserPlace(usersWithPlaces, leaderboardMinScores, user, place);
        }

        return usersWithPlaces;
    }

    private static int SetUserPlace(ICollection<UserWithPlace> usersWithPlaces,LeaderboardMinScores leaderboardMinScores, IUserWithScore user, int place)
    {
        var firstPlaceMiss = place == 1 && user.Score < leaderboardMinScores.FirstPlaceMinScore;
        var secondPlaceMiss = place == 2 && user.Score < leaderboardMinScores.SecondPlaceMinScore;
        var thirdPlaceMiss = place == 3 && user.Score < leaderboardMinScores.ThirdPlaceMinScore;
        if (firstPlaceMiss || secondPlaceMiss || thirdPlaceMiss)
        {
            place++;
            return SetUserPlace(usersWithPlaces, leaderboardMinScores, user, place);
        }

        usersWithPlaces.Add(new UserWithPlace(user.UserId, place));
        place++;
        return place;
    }
}