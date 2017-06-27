using System;

namespace SimpleVendingMachine
{
    public class CardAuthenticator
    {
        public bool Authenticate(VendingCard card, string pin)
        {
            if(card == null)
            {
                throw new ArgumentException("Card cannot be null.", "card");
            }

            if (pin == null)
            {
                throw new ArgumentException("Pin cannot be null.", "pin");
            }

            return card.VerifyPin(pin);
        }
    }
}
