var products = new List<Product>
{
    new Product(1, "Вода", 35, 10),
    new Product(2, "Чай", 45, 8),
    new Product(3, "Кофе", 55, 6),
    new Product(4, "Сок", 60, 5)
};

var bank = new Dictionary<int, int>
{
    {1, 50},
    {2, 20},
    {5, 10},
    {10, 10}
};

var inserted = CreateEmptyWallet();
const string AdminPin = "1234";

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Вендинговый автомат");
    Console.WriteLine("Баланс: " + WalletTotal(inserted) + " ₽");
    Console.WriteLine("1. Показать товары");
    Console.WriteLine("2. Вставить монету (1/2/5/10)");
    Console.WriteLine("3. Купить по ID");
    Console.WriteLine("4. Отмена покупки");
    Console.WriteLine("5. Админ-меню");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");
    var choice = Console.ReadLine();

    if (choice == "0")
    {
        break;
    }

    if (choice == "1")
    {
        PrintProducts(products);
        continue;
    }

    if (choice == "2")
    {
        Console.Write("Номинал монеты: ");
        if (int.TryParse(Console.ReadLine(), out var value) && inserted.ContainsKey(value))
        {
            inserted[value] += 1;
            Console.WriteLine($"Принято {value} ₽");
        }
        else
        {
            Console.WriteLine("Автомат принимает только 1, 2, 5 и 10 рублей.");
        }
        continue;
    }

    if (choice == "3")
    {
        Console.Write("ID товара: ");
        if (!int.TryParse(Console.ReadLine(), out var productId))
        {
            Console.WriteLine("Неверный ввод.");
            continue;
        }

        var product = FindProduct(products, productId);
        if (product == null)
        {
            Console.WriteLine("Товар с таким номером не найден.");
            continue;
        }

        if (product.Quantity <= 0)
        {
            Console.WriteLine("Товар закончился.");
            continue;
        }

        var balance = WalletTotal(inserted);
        if (balance < product.Price)
        {
            Console.WriteLine("Недостаточно средств.");
            continue;
        }

        var changeAmount = balance - product.Price;
        var tempBank = CopyWallet(bank);
        AddWallet(tempBank, inserted);

        if (!TryMakeChange(changeAmount, tempBank, out var change))
        {
            Console.WriteLine("Сдачу выдать не получается. Пополните банк или внесите точную сумму.");
            continue;
        }

        product.Quantity -= 1;
        AddWallet(bank, inserted);
        RemoveWallet(bank, change);
        if (changeAmount > 0)
        {
            PrintCoins(change, "Сдача: ");
        }
        else
        {
            Console.WriteLine("Спасибо за покупку!");
        }
        ClearWallet(inserted);
        continue;
    }

    if (choice == "4")
    {
        var refund = CopyWallet(inserted);
        if (WalletTotal(refund) == 0)
        {
            Console.WriteLine("Возвращать нечего.");
        }
        else
        {
            PrintCoins(refund, "Возьмите ваши монеты: ");
            ClearWallet(inserted);
        }
        continue;
    }

    if (choice == "5")
    {
        Console.Write("PIN: ");
        var pin = Console.ReadLine();
        if (pin != AdminPin)
        {
            Console.WriteLine("Неверный PIN.");
            continue;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Админ-меню");
            Console.WriteLine("1. Пополнить товар");
            Console.WriteLine("2. Добавить новый товар");
            Console.WriteLine("3. Пополнить монеты");
            Console.WriteLine("4. Показать банк");
            Console.WriteLine("5. Собрать наличные");
            Console.WriteLine("0. Назад");
            Console.Write("Выбор: ");
            var adminChoice = Console.ReadLine();

            if (adminChoice == "0" || string.IsNullOrEmpty(adminChoice))
            {
                break;
            }

            if (adminChoice == "1")
            {
                Console.Write("ID товара: ");
                if (!int.TryParse(Console.ReadLine(), out var id))
                {
                    Console.WriteLine("Неверный ввод.");
                    continue;
                }

                var product = FindProduct(products, id);
                if (product == null)
                {
                    Console.WriteLine("Нет товара с таким ID.");
                    continue;
                }

                Console.Write("На сколько пополнить: ");
                if (!int.TryParse(Console.ReadLine(), out var addQty) || addQty <= 0)
                {
                    Console.WriteLine("Количество должно быть больше нуля.");
                    continue;
                }

                product.Quantity += addQty;
                Console.WriteLine("Готово.");
                continue;
            }

            if (adminChoice == "2")
            {
                Console.Write("Новый ID: ");
                if (!int.TryParse(Console.ReadLine(), out var newId))
                {
                    Console.WriteLine("Неверный ввод.");
                    continue;
                }

                if (FindProduct(products, newId) != null)
                {
                    Console.WriteLine("Такой ID уже занят.");
                    continue;
                }

                Console.Write("Название: ");
                var name = Console.ReadLine() ?? string.Empty;
                Console.Write("Цена: ");
                if (!int.TryParse(Console.ReadLine(), out var price) || price <= 0)
                {
                    Console.WriteLine("Цена должна быть положительной.");
                    continue;
                }

                Console.Write("Количество: ");
                if (!int.TryParse(Console.ReadLine(), out var qty) || qty < 0)
                {
                    Console.WriteLine("Некорректное количество.");
                    continue;
                }

                products.Add(new Product(newId, name, price, qty));
                products.Sort((a, b) => a.Id.CompareTo(b.Id));
                Console.WriteLine("Товар добавлен.");
                continue;
            }

            if (adminChoice == "3")
            {
                Console.Write("Номинал монеты: ");
                if (!int.TryParse(Console.ReadLine(), out var value) || !bank.ContainsKey(value))
                {
                    Console.WriteLine("Можно пополнять только 1, 2, 5 или 10 рублей.");
                    continue;
                }

                Console.Write("Количество: ");
                if (!int.TryParse(Console.ReadLine(), out var qty) || qty <= 0)
                {
                    Console.WriteLine("Количество должно быть больше нуля.");
                    continue;
                }

                bank[value] += qty;
                Console.WriteLine("Монеты добавлены.");
                continue;
            }

            if (adminChoice == "4")
            {
                Console.WriteLine("В банке всего: " + WalletTotal(bank) + " ₽");
                PrintCoins(bank, "Монет по номиналам: ");
                continue;
            }

            if (adminChoice == "5")
            {
                var collected = WalletTotal(bank);
                var coinsToCollect = new[] { 1, 2, 5, 10 };
                foreach (var coin in coinsToCollect)
                {
                    if (bank.ContainsKey(coin))
                    {
                        bank[coin] = 0;
                    }
                }
                Console.WriteLine("Собрано: " + collected + " ₽");
                continue;
            }
        }
    }
}

static void PrintProducts(List<Product> products)
{
    Console.WriteLine("Доступные товары:");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Id}. {product.Name} — {product.Price} ₽ (осталось {product.Quantity})");
    }
}

static Product? FindProduct(List<Product> products, int id)
{
    foreach (var product in products)
    {
        if (product.Id == id)
        {
            return product;
        }
    }
    return null;
}

static Dictionary<int, int> CreateEmptyWallet()
{
    return new Dictionary<int, int>
    {
        {1, 0},
        {2, 0},
        {5, 0},
        {10, 0}
    };
}

static int WalletTotal(Dictionary<int, int> wallet)
{
    var total = 0;
    foreach (var pair in wallet)
    {
        total += pair.Key * pair.Value;
    }
    return total;
}

static void ClearWallet(Dictionary<int, int> wallet)
{
    var coins = new[] { 1, 2, 5, 10 };
    foreach (var coin in coins)
    {
        if (wallet.ContainsKey(coin))
        {
            wallet[coin] = 0;
        }
    }
}

static Dictionary<int, int> CopyWallet(Dictionary<int, int> source)
{
    var copy = new Dictionary<int, int>();
    foreach (var pair in source)
    {
        copy[pair.Key] = pair.Value;
    }
    return copy;
}

static void AddWallet(Dictionary<int, int> target, Dictionary<int, int> addition)
{
    foreach (var pair in addition)
    {
        if (!target.ContainsKey(pair.Key))
        {
            target[pair.Key] = 0;
        }
        target[pair.Key] += pair.Value;
    }
}

static void RemoveWallet(Dictionary<int, int> target, Dictionary<int, int> remove)
{
    foreach (var pair in remove)
    {
        if (target.ContainsKey(pair.Key))
        {
            target[pair.Key] = Math.Max(0, target[pair.Key] - pair.Value);
        }
    }
}

static bool TryMakeChange(int amount, Dictionary<int, int> bank, out Dictionary<int, int> change)
{
    change = new Dictionary<int, int>();
    if (amount == 0)
    {
        return true;
    }

    var order = new[] { 10, 5, 2, 1 };
    var remaining = amount;
    foreach (var coin in order)
    {
        if (!bank.ContainsKey(coin))
        {
            continue;
        }

        var available = bank[coin];
        var need = remaining / coin;
        if (need > available)
        {
            need = available;
        }

        if (need > 0)
        {
            change[coin] = need;
            remaining -= coin * need;
        }
    }

    if (remaining == 0)
    {
        return true;
    }

    change.Clear();
    return false;
}

static void PrintCoins(Dictionary<int, int> coins, string title)
{
    Console.Write(title);
    var order = new[] { 10, 5, 2, 1 };
    var parts = new List<string>();
    foreach (var coin in order)
    {
        if (coins.ContainsKey(coin) && coins[coin] > 0)
        {
            parts.Add($"{coin}×{coins[coin]}");
        }
    }
    if (parts.Count == 0)
    {
        Console.WriteLine("нет монет");
    }
    else
    {
        Console.WriteLine(string.Join(", ", parts));
    }
}

public class Product
{
    public Product(int id, string name, int price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}
