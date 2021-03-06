﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NacrtovalskiVzorci
{
    /** Primer je povzet po primeru iz https://dotnettutorials.net/lesson/factory-design-pattern-csharp/
     * 
     * Najprej predstavimo realizacijo rešitve brez uporabe vzorca factory, 
     * nato pa še z njim.
     */

    #region Brez uporabe vzorca factory

    /// <summary>
    /// Vmesnik, ki ga implementira razred CreditCard
    /// </summary>
    public interface ICreditCard
    {
        CreditCardType CreditCardType { get; }
        double Limit { get; }
        double AnnualCharge { get; }
    }

    public enum CreditCardType
    {
        Silver,
        Gold,
        Platinum,
        Student
    }

    class Silver : ICreditCard
    {
        public CreditCardType CreditCardType
        {
            get
            {
                return CreditCardType.Silver;
            }
        }

        public double Limit
        {
            get
            {
                return 800;
            }
        }

        public double AnnualCharge
        {
            get
            {
                return 20;
            }
        }
    }

    class Gold : ICreditCard
    {
        public CreditCardType CreditCardType
        {
            get
            {
                return CreditCardType.Gold;
            }
        }

        public double Limit
        {
            get
            {
                return 2000;
            }
        }

        public double AnnualCharge
        {
            get
            {
                return 50;
            }
        }
    }

    class Platinum : ICreditCard
    {
        public CreditCardType CreditCardType
        {
            get
            {
                return CreditCardType.Platinum;
            }
        }

        public double Limit
        {
            get
            {
                return 5000;
            }
        }

        public double AnnualCharge
        {
            get
            {
                return 100;
            }
        }
    }

    class Student : ICreditCard
    {
        public CreditCardType CreditCardType
        {
            get
            {
                return CreditCardType.Student;
            }
        }

        public double Limit
        {
            get
            {
                return 200;
            }
        }

        public double AnnualCharge
        {
            get
            {
                return 5;
            }
        }
    }

    #endregion


    #region Z uporabo vzorca factory

    /// <summary>
    /// Logiko iz uporabniškega vmesnika, ki se tiče izbire tipa kartice 
    /// prenesemo v razred CreditCardFactory.
    /// </summary>
    static class CreditCardFactory
    {
        public static ICreditCard GetCreditCard(CreditCardType type)
        {
            // Pripravimo si novo spremenljivko
            ICreditCard card = null;

            // Ustvarimo instanco glede na izbrani tip
            switch (type)
            {
                case CreditCardType.Student:
                    card = new Student();
                    break;
                case CreditCardType.Silver:
                    card = new Silver();
                    break;
                case CreditCardType.Gold:
                    card = new Gold();
                    break;
                case CreditCardType.Platinum:
                    card = new Platinum();
                    break;
            }

            return card;
        }
    }

    #endregion
}
