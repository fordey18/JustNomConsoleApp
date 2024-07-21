using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using JustNomConsoleApp;
using System.Globalization;

namespace JustNomConsoleApp;

public class Topping : Ingredient
{
    private string _name;
    private int _price;

    public Topping(string name, int price) : base(name, price)
    {
        _name = name;
        _price = price;

    }
}