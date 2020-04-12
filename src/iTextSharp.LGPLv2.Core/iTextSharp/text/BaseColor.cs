using System;

namespace iTextSharp.text
{
    /// <summary>
    /// Base class for Color, serves as wrapper class for <see cref="T:System.Drawing.Color"/>
    /// to allow extension.
    /// </summary>
    public class BaseColor
    {
        public static readonly BaseColor Black = new BaseColor(0, 0, 0);
        public static readonly BaseColor Blue = new BaseColor(0, 0, 255);
        public static readonly BaseColor Cyan = new BaseColor(0, 255, 255);
        public static readonly BaseColor DarkGray = new BaseColor(64, 64, 64);
        public static readonly BaseColor Gray = new BaseColor(128, 128, 128);
        public static readonly BaseColor Green = new BaseColor(0, 255, 0);
        public static readonly BaseColor LightGray = new BaseColor(192, 192, 192);
        public static readonly BaseColor Magenta = new BaseColor(255, 0, 255);
        public static readonly BaseColor Orange = new BaseColor(255, 200, 0);
        public static readonly BaseColor Pink = new BaseColor(255, 175, 175);
        public static readonly BaseColor Red = new BaseColor(255, 0, 0);
        public static readonly BaseColor White = new BaseColor(255, 255, 255);
        public static readonly BaseColor Yellow = new BaseColor(255, 255, 0);
        private const double Factor = 0.7;
        private readonly System.Drawing.Color _color;

        /// <summary>
        /// Constructor for Color object.
        /// </summary>
        /// <param name="red">The red component value for the new Color structure. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the new Color structure. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new Color structure. Valid values are 0 through 255.</param>
        public BaseColor(int red, int green, int blue)
        {
            _color = System.Drawing.Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Constructor for Color object.
        /// </summary>
        /// <param name="red">The red component value for the new Color structure. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the new Color structure. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new Color structure. Valid values are 0 through 255.</param>
        /// <param name="alpha">The transparency component value for the new Color structure. Valid values are 0 through 255.</param>
        public BaseColor(int red, int green, int blue, int alpha)
        {
            _color = System.Drawing.Color.FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Constructor for Color object
        /// </summary>
        /// <param name="red">The red component value for the new Color structure. Valid values are 0 through 1.</param>
        /// <param name="green">The green component value for the new Color structure. Valid values are 0 through 1.</param>
        /// <param name="blue">The blue component value for the new Color structure. Valid values are 0 through 1.</param>
        public BaseColor(float red, float green, float blue)
        {
            _color = System.Drawing.Color.FromArgb((int)(red * 255 + .5), (int)(green * 255 + .5), (int)(blue * 255 + .5));
        }

        /// <summary>
        /// Constructor for Color object
        /// </summary>
        /// <param name="red">The red component value for the new Color structure. Valid values are 0 through 1.</param>
        /// <param name="green">The green component value for the new Color structure. Valid values are 0 through 1.</param>
        /// <param name="blue">The blue component value for the new Color structure. Valid values are 0 through 1.</param>
        /// <param name="alpha">The transparency component value for the new Color structure. Valid values are 0 through 1.</param>
        public BaseColor(float red, float green, float blue, float alpha)
        {
            _color = System.Drawing.Color.FromArgb((int)(alpha * 255 + .5), (int)(red * 255 + .5), (int)(green * 255 + .5), (int)(blue * 255 + .5));
        }

        public BaseColor(int argb)
        {
            _color = System.Drawing.Color.FromArgb(argb);
        }

        /// <summary>
        /// Constructor for Color object
        /// </summary>
        /// <param name="color">a Color object</param>
        /// <overloads>
        /// Has three overloads.
        /// </overloads>
        public BaseColor(System.Drawing.Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Gets the blue component value of this <see cref="T:System.Drawing.Color"/> structure.
        /// </summary>
        /// <value>The blue component value of this <see cref="T:System.Drawing.Color"/> structure.</value>
        public int B => _color.B;

        /// <summary>
        /// Gets the green component value of this <see cref="T:System.Drawing.Color"/> structure.
        /// </summary>
        /// <value>The green component value of this <see cref="T:System.Drawing.Color"/> structure.</value>
        public int G => _color.G;

        /// <summary>
        /// Gets the red component value of this <see cref="T:System.Drawing.Color"/> structure.
        /// </summary>
        /// <value>The red component value of this <see cref="T:System.Drawing.Color"/> structure.</value>
        public int R => _color.R;

        public BaseColor Brighter()
        {
            int r = _color.R;
            int g = _color.G;
            int b = _color.B;

            var i = (int)(1.0 / (1.0 - Factor));
            if (r == 0 && g == 0 && b == 0)
            {
                return new BaseColor(i, i, i);
            }
            if (r > 0 && r < i)
            {
                r = i;
            }

            if (g > 0 && g < i)
            {
                g = i;
            }

            if (b > 0 && b < i)
            {
                b = i;
            }

            return new BaseColor(Math.Min((int)(r / Factor), 255),
                    Math.Min((int)(g / Factor), 255),
                    Math.Min((int)(b / Factor), 255));
        }

        public BaseColor Darker()
        {
            return new BaseColor(Math.Max((int)(_color.R * Factor), 0),
                    Math.Max((int)(_color.G * Factor), 0),
                    Math.Max((int)(_color.B * Factor), 0));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseColor))
            {
                return false;
            }

            return _color.Equals(((BaseColor)obj)._color);
        }

        public override int GetHashCode()
        {
            return _color.GetHashCode();
        }

        public int ToArgb()
        {
            return _color.ToArgb();
        }
    }
}
