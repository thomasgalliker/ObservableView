﻿using System.Globalization;


namespace ObservableView.Tests.TestHelpers
{
    /// <summary>
    /// Apply this attribute to your test method to replace the
    /// <see cref="Thread.CurrentThread" /> <see cref="CultureInfo.CurrentCulture" /> and
    /// <see cref="CultureInfo.CurrentUICulture" /> with another culture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UseCultureAttribute : BeforeAfterTestAttribute
    {
        private readonly Lazy<CultureInfo> culture;
        private readonly Lazy<CultureInfo> uiCulture;

        private CultureInfo originalCulture;
        private CultureInfo originalUiCulture;

        /// <summary>
        /// Replaces the culture and UI culture of the current thread with
        /// <paramref name="culture" />
        /// </summary>
        /// <param name="culture">The name of the culture.</param>
        /// <remarks>
        /// <para>
        /// This constructor overload uses <paramref name="culture" /> for both
        /// <see cref="Culture" /> and <see cref="UiCulture" />.
        /// </para>
        /// </remarks>
        public UseCultureAttribute(string culture)
            : this(culture, culture) { }

        /// <summary>
        /// Replaces the culture and UI culture of the current thread with
        /// <paramref name="culture" /> and <paramref name="uiCulture" />
        /// </summary>
        /// <param name="culture">The name of the culture.</param>
        /// <param name="uiCulture">The name of the UI culture.</param>
        public UseCultureAttribute(string culture, string uiCulture)
        {
            this.culture = new Lazy<CultureInfo>(() => new CultureInfo(culture));
            this.uiCulture = new Lazy<CultureInfo>(() => new CultureInfo(uiCulture));
        }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        public CultureInfo Culture => this.culture.Value;

        /// <summary>
        /// Gets the UI culture.
        /// </summary>
        public CultureInfo UiCulture => this.uiCulture.Value;

        /// <summary>
        /// Stores the current <see cref="Thread.CurrentPrincipal" />
        /// <see cref="CultureInfo.CurrentCulture" /> and <see cref="CultureInfo.CurrentUICulture" />
        /// and replaces them with the new cultures defined in the constructor.
        /// </summary>
        /// <param name="methodUnderTest">The method under test</param>
        public override void Before(MethodInfo methodUnderTest)
        {
            this.originalCulture = Thread.CurrentThread.CurrentCulture;
            this.originalUiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = this.Culture;
            Thread.CurrentThread.CurrentUICulture = this.UiCulture;
        }

        /// <summary>
        /// Restores the original <see cref="CultureInfo.CurrentCulture" /> and
        /// <see cref="CultureInfo.CurrentUICulture" /> to <see cref="Thread.CurrentPrincipal" />
        /// </summary>
        /// <param name="methodUnderTest">The method under test</param>
        public override void After(MethodInfo methodUnderTest)
        {
            Thread.CurrentThread.CurrentCulture = this.originalCulture;
            Thread.CurrentThread.CurrentUICulture = this.originalUiCulture;
        }
    }
}