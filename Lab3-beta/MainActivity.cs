using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Xml.Serialization;

namespace Lab3
{
    [Activity(Label = "Lab3", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        QuoteBank quoteCollection;
        TextView quotationTextView;
        TextView quotationTextViewPerson;

     

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);


            quotationTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            quotationTextViewPerson = FindViewById<TextView>(Resource.Id.quoteTextViewPerson);
           
            if (savedInstanceState != null)
            {
                //Save the state of the current quotes including the new quotes that are added
                string quotes = savedInstanceState.GetString("Quotes");
                XmlSerializer serializer = new XmlSerializer(typeof(QuoteBank));
                quoteCollection = (QuoteBank)serializer.Deserialize(new StringReader(quotes));
            }
            else {
                quoteCollection = new QuoteBank();
                quoteCollection.LoadQuotes();
                quoteCollection.GetNextQuote();
            }

            quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
            quotationTextViewPerson.Text = quoteCollection.CurrentQuote.Person;


            // Display another quote when the "Next Quote" button is tapped
            var nextButton = FindViewById<Button>(Resource.Id.nextButton);
            nextButton.Click += delegate {
                quoteCollection.GetNextQuote();
                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
                quotationTextViewPerson.Text = quoteCollection.CurrentQuote.Person;
            };

            var enterButton = FindViewById<Button>(Resource.Id.enterQuote);
            enterButton.Click += delegate
            {
                var quote = FindViewById<EditText>(Resource.Id.newText);
                var author = FindViewById<EditText>(Resource.Id.byText);
                var quoteText = FindViewById<TextView>(Resource.Id.quoteTextView);
                var quoteTextPerson = FindViewById<TextView>(Resource.Id.quoteTextViewPerson);

                //create a new quote with the text that was entered by the user
                quoteCollection.Quotes.Add(new Quote { Quotation = quote.Text, Person = author.Text });
                quoteCollection.GetEnteredQuote();

                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
                quotationTextViewPerson.Text = quoteCollection.CurrentQuote.Person;

                quote.Text = "";
                author.Text = "";

            };

          
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
       
            StringWriter text = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(quoteCollection.GetType());
            serializer.Serialize(text, quoteCollection);
            string QuoteCollection = text.ToString();
            outState.PutString("Quotes", QuoteCollection);


        }
    }
}

