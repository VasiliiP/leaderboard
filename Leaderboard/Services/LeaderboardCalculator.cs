﻿using Leaderboard.Models;

/**
 * ТРЕБОВАНИЕ:
 *
 * Необходимо реализовать функцию CalculatePlaces.
 * Функция распределяет места пользователей, учитывая ограничения для получения первых мест и набранные пользователями очки.
 * Подробное ТЗ смотреть в readme.txt
 */

/**
 * ТЕХНИЧЕСКИЕ ОГРАНИЧЕНИЯ:
 *
 * количество очков это всегда целое положительное число
 * FirstPlaceMinScore > SecondPlaceMinScore > ThirdPlaceMinScore > 0
 * в конкурсе участвует от 1 до 100 пользователей
 * 2 пользователя не могут набрать одинаковое количество баллов (разные баллы у пользователей гарантируются бизнес-логикой, не стоит усложнять алгоритм)
 * нет ограничений на скорость работы функции и потребляемую ей память
 * при реализации функции разрешается использование любых библиотек, любого стиля написания кода
 * в функцию передаются только валидные данные, которые соответствуют предыдущим ограничениям (проверять это в функции не нужно)
 */

/**
 * ВХОДНЫЕ ДАННЫЕ:
 *
 * usersWithScores - это список пользователей и заработанные каждым из них очки,
 * это неотсортированный массив вида [{userId: "id1", score: score1}, ... , {userId: "idn", score: scoreN}], где score1 ... scoreN различные положительные целые числа, id1 ... idN произвольные неповторяющиеся идентификаторы
 *
 * leaderboardMinScores - это значения минимального количества очков для первых 3 мест
 * это объект вида { FirstPlaceMinScore: score1, SecondPlaceMinScore: score2, ThirdPlaceMinScore : score3 }, где score1 > score2 > score3 > 0 целые положительные числа
 */

/**
 * РЕЗУЛЬТАТ:
 *
 * Функция должна вернуть пользователей с занятыми ими местами
 * Массив вида (сортировка массива не важна): [{UserId: "id1", Place: user1Place}, ..., {UserId: "idN", Place: userNPlace}], где user1Place ... userNPlace это целые положительные числа равные занятым пользователями местами, id1 ... idN идентификаторы пользователей из массива users
 */


namespace Leaderboard.Services
{
    public class LeaderboardCalculator : ILeaderboardCalculator
    {
        public IEnumerable<UserWithPlace> CalculatePlaces(IEnumerable<IUserWithScore> usersWithScores,
            LeaderboardMinScores leaderboardMinScores)
        {
            var users = usersWithScores.ToList();

            var sortedDict = new SortedDictionary<int, long>(new DescendingComparer<int>());
            
            sortedDict.Add(leaderboardMinScores.FirstPlaceMinScore, -1);
            sortedDict.Add(leaderboardMinScores.SecondPlaceMinScore, -1);
            sortedDict.Add(leaderboardMinScores.ThirdPlaceMinScore, -1);

            foreach (var user in users)
            {
                if (sortedDict.TryAdd(user.Score, user.UserId)) continue;
                sortedDict.Remove(user.Score);
                sortedDict.Add(user.Score, user.UserId);
            }

            var place = 1;
            var sortedUsers = new List<UserWithPlace>(sortedDict.Count);
            
            foreach (var userId in sortedDict.Values)
            {
                if (userId < 0)
                {
                    if (place <= 3)
                    {
                        place++;
                    }
                    continue;
                }

                sortedUsers.Add(new UserWithPlace(userId, place));
                place++;
            }

            return sortedUsers;
        }
    }
    
    public class DescendingComparer<T> : IComparer<T> where T : IComparable<T> 
    {
        public int Compare(T x, T y) 
        {
            return y.CompareTo(x);
        }
    }
}