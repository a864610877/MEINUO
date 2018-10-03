using System;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Ecard.Services;

namespace Ecard.Mvc.Models
{
    public class AccountDependency
    {
        private readonly IAccountDependency _accountDependency;
        public bool[] WeekDays { get; set; }
        public AccountDependency(IAccountDependency accountDependency)
        {
            _accountDependency = accountDependency;
            WeekDays = new bool[7];
            if (!string.IsNullOrWhiteSpace(accountDependency.WeekDays))
            {
                var days = accountDependency.WeekDays.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();

                foreach (var day in days)
                {
                    if (day <= 7 && day > 0)
                        WeekDays[day - 1] = true;
                }
            }
        }

        public bool Monday
        {
            get { return WeekDays[0]; }
            set { WeekDays[0] = value; }
        }
        public bool Tuesday
        {
            get { return WeekDays[1]; }
            set { WeekDays[1] = value; }
        }
        public bool Wednesday
        {
            get { return WeekDays[2]; }
            set { WeekDays[2] = value; }
        }
        public bool Thursday
        {
            get { return WeekDays[3]; }
            set { WeekDays[3] = value; }
        }
        public bool Friday
        {
            get { return WeekDays[4]; }
            set { WeekDays[4] = value; }
        }
        public bool Saturday
        {
            get { return WeekDays[5]; }
            set { WeekDays[5] = value; }
        }
        public bool Sunday
        {
            get { return WeekDays[6]; }
            set { WeekDays[6] = value; }
        }
        public string Days
        {
            get { return _accountDependency.Days; }
            set { _accountDependency.Days = value; }
        }

        public bool EnableEveryDay
        {
            get { return (_accountDependency.DependencyType & AccountDependencyTypes.EveryDay) != 0; }
            set
            {
                if (value)
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType | AccountDependencyTypes.EveryDay);
                else
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType & ~AccountDependencyTypes.EveryDay);
            }
        }
        public bool EnableDay
        {
            get { return (_accountDependency.DependencyType & AccountDependencyTypes.Day) != 0; }
            set
            {
                if (value)
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType | AccountDependencyTypes.Day);
                else
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType & ~AccountDependencyTypes.Day);
            }
        }
        public bool EnableBirthDate
        {
            get { return (_accountDependency.DependencyType & AccountDependencyTypes.BirthDate) != 0; }
            set
            {
                if (value)
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType | AccountDependencyTypes.BirthDate);
                else
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType & ~AccountDependencyTypes.BirthDate);
            }
        }
        public bool EnableWeekly
        {
            get { return (_accountDependency.DependencyType & AccountDependencyTypes.Weekly) != 0; }
            set
            {
                if (value)
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType | AccountDependencyTypes.Weekly);
                else
                    _accountDependency.DependencyType = (int)(_accountDependency.DependencyType & ~AccountDependencyTypes.Weekly);
            }
        } 
        public void Save(IAccountDependency accountDependency)
        {
            List<string> weeklyDays = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                if (WeekDays[i])
                    weeklyDays.Add((i + 1).ToString());
            }
            accountDependency.WeekDays = string.Join(",", weeklyDays.ToArray());
            accountDependency.DependencyType = _accountDependency.DependencyType;
            accountDependency.Days = _accountDependency.Days;
        }
    }
}