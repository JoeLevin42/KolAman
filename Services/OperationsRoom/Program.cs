// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
public async Task<List<Book>> GetAsync(string authorName) => 
    await _booksCollection.Find(book => book.Author == authorName).ToListAsync();

public async Task<List<Book>> GetAsync() =>
    await booksCollection.Find(book => book.Status == "wait").ToListAsync();

public async Task<List<Book>> GetAsync()
{
    var filter = Builders<Book>.Filter.Eq(book => book.Status, "wait");
    return await booksCollection.Find(filter).ToListAsync();
}

//var commonTitles = new List<string>();

var lists = new[] { list1, list2, list3, list4 };

foreach (var list in lists)
{
    foreach (var alert in list)
    {
        int count = lists.Count(x => x.Any(a => a.Title == alert.Title));

        if (count > 1 && !commonTitles.Contains(alert.Title))
            commonTitles.Add(alert.Title);
    }
}