﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cats.Models.Hubs.ViewModels
{
    public class UserPreferenceViewModel
    {
        public List<LookupViewModel> Languages { get; set; }
        public List<LookupViewModel> DateFormatPreferences { get; set; }
        public List<LookupViewModel> WeightPerferences { get; set;}
        public List<LookupViewModel> ThemePreferences { get; set; }

        public string Language { get; set; }
        public string WeightPrefernce { get; set; }
        public string ThemePreference { get; set; }
        public string DateFormatPreference { get; set; }

        public UserPreferenceViewModel()
        {
            //InitializeLookupValues();
        }

        private void InitializeLookupValues()
        {
            // initalize the languages
            Languages = new List<LookupViewModel>
                            {
                                new LookupViewModel() {Name = "English", StringID = "en"},
                                new LookupViewModel() {Name = "Amharic", StringID = "am"}
                            };
            DateFormatPreferences = new List<LookupViewModel>
                                        {
                                            new LookupViewModel() {Name = "Gregorian", StringID = "en"},
                                            new LookupViewModel() {Name = "Ethiopian", StringID = "am"}
                                        };
            WeightPerferences = new List<LookupViewModel>
                                    {
                                        new LookupViewModel() {Name = "Quintals", StringID = "qn"},
                                        new LookupViewModel() {Name = "Metric Tonne", StringID = "mt"}
                                    };
            ThemePreferences = new List<LookupViewModel>
                                   {
                                       new LookupViewModel(){Name = "Metro",StringID = "metro"},
                                       new LookupViewModel(){Name = "Vista",StringID = "vista"},
                                       new LookupViewModel(){Name = "Forest",StringID = "forest"},
                                       new LookupViewModel(){Name = "Windows 7",StringID = "windows7"},
                                       new LookupViewModel(){Name = "Simple",StringID = "simple"},
                                       new LookupViewModel(){Name = "Hay",StringID = "hay"},
                                       new LookupViewModel(){Name = "Black",StringID = "black"},
                                       new LookupViewModel(){Name = "Default",StringID = "default"},
                                       new LookupViewModel(){Name = "Web Blue",StringID = "webblue"},
                                       new LookupViewModel(){Name = "Transparent",StringID = "transparent"},
                                       new LookupViewModel(){Name = "Web 2.0",StringID = "web20"},
                                       new LookupViewModel(){Name = "Sitefinity",StringID = "sitefinity"},
                                   };
        }

        public UserPreferenceViewModel(UserProfile profile)
        {
            InitializeLookupValues();
            this.DateFormatPreference = profile.DatePreference;
            this.Language = profile.LanguageCode;
            this.WeightPrefernce = profile.PreferedWeightMeasurment;
            this.ThemePreference = profile.DefaultTheme;
        }
    }
}
