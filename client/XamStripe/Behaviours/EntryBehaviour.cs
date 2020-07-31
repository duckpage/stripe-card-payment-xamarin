using Xamarin.Forms;

namespace XamStripe.Behaviours
{
    public class EntryBehaviour : Behavior<Entry>
    {
        public string Mask { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = (Entry)sender;
            var oldString = args.OldTextValue;
            var newString = args.NewTextValue;
            string entryText = entry.Text;

            if (Mask != null && Mask.Length > 0 && entryText.Length > 0)
            {
                var output = ProcessMask(entryText, oldString, newString, Mask);
                if (output != entryText)
                {
                    entryText = output;
                    entry.Text = entryText;
                    return;
                }
            }

            entry.Text = entryText;
        }

        string ProcessMask(string entryText, string oldString, string newString, string mask)
        {
            string output = entryText;

            if (oldString != null)
            {
                if (returnCheck(oldString, mask))
                {
                    if (newString.Length > oldString.Length)
                    {
                        if (mask.Length >= newString.Length)
                        {
                            var ln = entryText.Length - 1;
                            var st = mask.Substring(ln, 1);
                            string newstr = "";
                            if (oldString.Length > 0)
                                newstr = newString.Substring(newString.Length - 1);
                            else
                                newstr = newString;
                            if (output.Length > 1)
                                output = output.Remove(output.Length - 1, 1);
                            else
                                output = "";
                            if (st == "#")
                            {
                                output = output + newstr;
                            }
                            else
                            {
                                foreach (var s in mask.Substring(ln))
                                {
                                    if (s == '#')
                                    {
                                        output = output + newstr;
                                        break;
                                    }
                                    else
                                    {
                                        output = output + s;
                                    }
                                }
                            }
                        }
                        else
                        {
                            output = oldString;
                        }
                    }
                }
            }

            return output;
        }


        bool returnCheck(string oldValue, string mask)
        {

            int i = 0;
            if (mask.Length < oldValue.Length)
            {
                return false;
            }

            foreach (var s in mask.Substring(0, oldValue.Length))
            {
                if (s.ToString() != "#" && s.ToString() != oldValue.Substring(i, 1))
                {
                    return false;
                }
                i++;
            }
            return true;
        }
    }
}
