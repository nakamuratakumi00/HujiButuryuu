namespace MacssWeb.Common
{
    public class ButtonUtil
    {
        public static ButtonValues RetrieveSubmitButton(params ButtonValues?[] buttons)
        {
            if (buttons == null || buttons.Length <= 0)
            {
                return ButtonValues.Nothing;
            }

            foreach (var item in buttons)
            {
                if (!item.HasValue)
                {
                    continue;
                }

                return item.Value;
            }

            return ButtonValues.Nothing;
        }
    }
}