using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace Dlw.EpiBase.Content.Convertors
{
    // not all cultures supported on azure
    // https://stackoverflow.com/questions/41851613/culture-is-suddenly-not-supported-anymore-on-azure-web-app
    public class CulturesRegionCodeConvertor : IRegionCodeConvertor
    {
        private readonly IDictionary<string, string> _twoLetter;
        private readonly IDictionary<string, string> _threeLetter;

        public CulturesRegionCodeConvertor()
        {
            _twoLetter = new ConcurrentDictionary<string, string>();
            _threeLetter = new ConcurrentDictionary<string, string>();

            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var culture in cultures)
            {
                var region = new RegionInfo(culture.Name);

                var twoLetter = region.TwoLetterISORegionName.ToUpperInvariant();
                var threeLetter = region.ThreeLetterISORegionName.ToUpperInvariant();

                if (!_twoLetter.ContainsKey(twoLetter))
                    _twoLetter.Add(twoLetter, threeLetter);

                if (!_threeLetter.ContainsKey(threeLetter))
                    _threeLetter.Add(threeLetter, twoLetter);
            }
        }

        public string ConvertFromThreeToTwo(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            if (code.Length != 3)
            {
                throw new ArgumentException("Code must be three letters.");
            }

            code = code.ToUpper();

            if (_threeLetter.ContainsKey(code)) return _threeLetter[code];

            return null;
        }

        public string ConvertFromTwoToThree(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            if (code.Length != 2)
            {
                throw new ArgumentException("Code must be two letters.");
            }

            code = code.ToUpper();

            if (_twoLetter.ContainsKey(code)) return _twoLetter[code];

            return null;
        }
    }
}