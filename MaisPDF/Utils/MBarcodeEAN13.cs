using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;

using System;

namespace MaisPDF
{
    public class MBarcodeEAN13 : BarCode
    {
        private XRect m_leftBlock = new XRect();
        private XRect m_rightBlock = new XRect();

        public MBarcodeEAN13() : base("", XSize.Empty, CodeDirection.LeftToRight)
        {
        }

        public MBarcodeEAN13(string code) : base(code, XSize.Empty, CodeDirection.LeftToRight)
        {
        }

        public MBarcodeEAN13(string code, XSize size) : base(code, size, CodeDirection.LeftToRight)
        {
        }

        static bool[] Quite = new bool[]
        {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false
        };

        static bool[] Leading = new bool[]
        {
            true,
            false,
            true
        };

        static bool[] Separator = new bool[]
        {
            false,
            true,
            false,
            true,
            false
        };

        static bool[][] OddLeftLines = new bool[][]
        {
            new bool[]
            {
                false,
                false,
                false,
                true,
                true,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                true,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                false,
                false,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                true,
                true,
                false,
                true
            },
            new bool[]
            {
                false,
                true,
                false,
                false,
                false,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                false,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                true,
                false,
                true,
                true,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                true,
                false,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                false,
                true,
                true,
                true
            },
            new bool[]
            {
                false,
                false,
                false,
                true,
                false,
                true,
                true
            }
        };

        static bool[][] EvenLeftLines = new bool[][]
        {
            new bool[]
            {
                false,
                true,
                false,
                false,
                true,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                false,
                false,
                true,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                true,
                false,
                true,
                true
            },
            new bool[]
            {
                false,
                true,
                false,
                false,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                true,
                true,
                false,
                true
            },
            new bool[]
            {
                false,
                true,
                true,
                true,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                false,
                false,
                true,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                false,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                false,
                true,
                false,
                false,
                true
            },
            new bool[]
            {
                false,
                false,
                true,
                false,
                true,
                true,
                true
            }
        };

        static bool[][] RightLines = new bool[][]
        {
            new bool[]
            {
                true,
                true,
                true,
                false,
                false,
                true,
                false
            },
            new bool[]
            {
                true,
                true,
                false,
                false,
                true,
                true,
                false
            },
            new bool[]
            {
                true,
                true,
                false,
                true,
                true,
                false,
                false
            },
            new bool[]
            {
                true,
                false,
                false,
                false,
                false,
                true,
                false
            },
            new bool[]
            {
                true,
                false,
                true,
                true,
                true,
                false,
                false
            },
            new bool[]
            {
                true,
                false,
                false,
                true,
                true,
                true,
                false
            },
            new bool[]
            {
                true,
                false,
                true,
                false,
                false,
                false,
                false
            },
            new bool[]
            {
                true,
                false,
                false,
                false,
                true,
                false,
                false
            },
            new bool[]
            {
                true,
                false,
                false,
                true,
                false,
                false,
                false
            },
            new bool[]
            {
                true,
                true,
                true,
                false,
                true,
                false,
                false
            }
        };

        protected override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            XGraphicsState state = gfx.Save();
            BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
            InitRendering(info);
            info.CurrPosInString = 0;
            info.CurrPos = position - CodeBase.CalcDistance(AnchorType.TopLeft, Anchor, Size);

            int numberOfBars = 12;
            numberOfBars *= 7;
            numberOfBars += 2 * (Quite.Length + Leading.Length);
            numberOfBars += Separator.Length;
            info.ThinBarWidth = ((double)this.Size.Width / (double)numberOfBars);
            RenderStart(info);
            m_leftBlock.X = info.CurrPos.X + info.ThinBarWidth / 2;
            RenderLeft(info);
            m_leftBlock.Width = info.CurrPos.X - m_leftBlock.Left;
            RenderSeparator(info);
            m_rightBlock.X = info.CurrPos.X;
            RenderRight(info);
            m_rightBlock.Width = info.CurrPos.X - m_rightBlock.Left - info.ThinBarWidth / 2;
            RenderStop(info);

            if (this.TextLocation == TextLocation.BelowEmbedded)
                RenderText(info);

            gfx.Restore(state);
        }

        internal void InitRendering(BarCodeRenderInfo info)
        {
            info.BarHeight = Size.Height;

            if (TextLocation != TextLocation.None)
                info.BarHeight *= 4.0 / 5;

            //#if DEBUG
            //    info.Gfx.DrawRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.LightGray)), new XRect(info.Position.X, info.Position.Y, Size.Width, Size.Height));
            //#endif

            switch (Direction) //-V3002
            {
                case CodeDirection.RightToLeft:
                    info.Gfx.RotateAtTransform(180, info.Position);

                    break;
                case CodeDirection.TopToBottom:
                    info.Gfx.RotateAtTransform(90, info.Position);

                    break;
                case CodeDirection.BottomToTop:
                    info.Gfx.RotateAtTransform(-90, info.Position);

                    break;
            }
        }

        private void RenderStart(BarCodeRenderInfo info)
        {
            RenderValue(info, Quite);
            RenderValue(info, Leading);
        }

        private void RenderLeft(BarCodeRenderInfo info)
        {
            int country = (int)(Text[0] - '0');
            string text = Text.Substring(1, 6);

            switch (country)
            {
                case 0:

                    foreach (char ch in text)
                        RenderDigit(info, ch, OddLeftLines);

                    break;
                case 1:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], OddLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], OddLeftLines);
                    RenderDigit(info, text[4], EvenLeftLines);
                    RenderDigit(info, text[5], EvenLeftLines);

                    break;
                case 2:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], OddLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], EvenLeftLines);
                    RenderDigit(info, text[4], OddLeftLines);
                    RenderDigit(info, text[5], EvenLeftLines);

                    break;
                case 3:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], OddLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], EvenLeftLines);
                    RenderDigit(info, text[4], EvenLeftLines);
                    RenderDigit(info, text[5], OddLeftLines);

                    break;
                case 4:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], OddLeftLines);
                    RenderDigit(info, text[3], OddLeftLines);
                    RenderDigit(info, text[4], EvenLeftLines);
                    RenderDigit(info, text[5], EvenLeftLines);

                    break;
                case 5:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], OddLeftLines);
                    RenderDigit(info, text[4], OddLeftLines);
                    RenderDigit(info, text[5], EvenLeftLines);

                    break;
                case 6:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], EvenLeftLines);
                    RenderDigit(info, text[4], OddLeftLines);
                    RenderDigit(info, text[5], OddLeftLines);

                    break;
                case 7:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], OddLeftLines);
                    RenderDigit(info, text[3], EvenLeftLines);
                    RenderDigit(info, text[4], OddLeftLines);
                    RenderDigit(info, text[5], EvenLeftLines);

                    break;
                case 8:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], OddLeftLines);
                    RenderDigit(info, text[3], EvenLeftLines);
                    RenderDigit(info, text[4], EvenLeftLines);
                    RenderDigit(info, text[5], OddLeftLines);

                    break;
                case 9:
                    RenderDigit(info, text[0], OddLeftLines);
                    RenderDigit(info, text[1], EvenLeftLines);
                    RenderDigit(info, text[2], EvenLeftLines);
                    RenderDigit(info, text[3], OddLeftLines);
                    RenderDigit(info, text[4], EvenLeftLines);
                    RenderDigit(info, text[5], OddLeftLines);

                    break;
            }
        }

        private void RenderDigit(BarCodeRenderInfo info, char digit, bool[][] lines)
        {
            int index = digit - '0';
            RenderValue(info, lines[index]);
        }

        private void RenderSeparator(BarCodeRenderInfo info)
        {
            RenderValue(info, Separator);
        }

        private void RenderRight(BarCodeRenderInfo info)
        {
            string text = Text.Substring(7);

            if (Text.Length == 12)
                text += CalculateChecksumDigit(Text);

            foreach (char ch in text)
                RenderDigit(info, ch, RightLines);
        }

        private void RenderStop(BarCodeRenderInfo info)
        {
            RenderValue(info, Leading);
            RenderValue(info, Quite);
        }

        private void RenderValue(BarCodeRenderInfo info, bool[] value)
        {
            foreach (bool bar in value)
            {
                if (bar)
                {
                    XRect rect = new XRect(info.CurrPos.X, info.CurrPos.Y, info.ThinBarWidth, Size.Height);
                    info.Gfx.DrawRectangle(info.Brush, rect);
                }

                info.CurrPos.X += info.ThinBarWidth;
            }
        }

        private void RenderText(BarCodeRenderInfo info)
        {
            if (info.Font == null)
                info.Font = new XFont("Courier New", Size.Height / 4);

            XPoint center = info.Position + CodeBase.CalcDistance(Anchor, AnchorType.TopLeft, Size);
            XSize textSize = info.Gfx.MeasureString(Text, info.Font);
            double height = textSize.Height;
            double y = info.Position.Y + Size.Height - textSize.Height;
            m_leftBlock.Height = height + 1;
            m_leftBlock.Y = y;
            m_rightBlock.Height = height + 1;
            m_rightBlock.Y = y;
            XPoint pos = new XPoint(info.Position.X, y);
            XGraphicsPath path = new XGraphicsPath();
            path.AddString(Text.Substring(0, 1), info.Font.FontFamily, info.Font.Style, info.Font.Size, pos, XStringFormats.TopLeft);
            info.Gfx.DrawRectangle(XBrushes.White, m_leftBlock);
            path.AddString(Text.Substring(1, 6), info.Font.FontFamily, info.Font.Style, info.Font.Size, m_leftBlock, XStringFormats.TopCenter);
            info.Gfx.DrawRectangle(XBrushes.White, m_rightBlock);
            string text = Text.Substring(7);

            if (Text.Length == 12)
                text += CalculateChecksumDigit(Text);

            pos = new XPoint(m_rightBlock.Left, m_rightBlock.Y);
            path.AddString(text, info.Font.FontFamily, info.Font.Style, info.Font.Size, m_rightBlock, XStringFormats.TopCenter);
            info.Gfx.DrawPath(null, info.Brush, path);
        }

        private string CalculateChecksumDigit(string text)
        {
            bool odd = false;
            int sum = 0;

            foreach (char ch in text)
            {
                sum += (ch - '0') * (odd ? 3 : 1);
                odd = !odd;
            }

            int result = (10 - (sum % 10)) % 10;

            return result.ToString();
        }

        protected override void CheckCode(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Parameter text (string) can not be null");

            if (text.Length != 13)
                throw new ArgumentException("Parameter text (string) can not have more or less than 12 characters");
        }
    }

    class BarCodeRenderInfo
    {
        public BarCodeRenderInfo(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            Gfx = gfx;
            Brush = brush;
            Font = font;
            Position = position;
        }

        public XGraphics Gfx;
        public XBrush Brush;
        public XFont Font;
        public XPoint Position;
        public double BarHeight;
        public XPoint CurrPos;
        public int CurrPosInString;
        public double ThinBarWidth;
    }
}
