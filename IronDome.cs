using IronDomeApi.Models;
using System.Reflection;

namespace IronDomeApi
{
    public class IronDome
    {
        public async static Task<bool> HandleAttack(Attack attack)
        {

            await Task.Delay(5000);

            Random random = new Random();
            bool intercepted = random.Next(0, 2) == 1;
            return intercepted;

        }
    }
}
