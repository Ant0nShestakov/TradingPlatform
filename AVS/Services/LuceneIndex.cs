using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using AVS.Models.AdvertisementModels;
using Lucene.Net.Analysis;

namespace AVS.Services
{
    public class LuceneIndex : IDisposable
    {

        private readonly LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        private readonly string IndexPath;
        private readonly Analyzer Analyzer;
        private readonly Lucene.Net.Store.Directory Directory;

        public LuceneIndex()
        {
            IndexPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LuceneIndex");
            Analyzer = new StandardAnalyzer(AppLuceneVersion);
            Directory = FSDirectory.Open(IndexPath);

            // Проверяем существование директории индекса
            if (!System.IO.Directory.Exists(IndexPath))
            {
                // Если не существует, создаем ее
                System.IO.Directory.CreateDirectory(IndexPath);
            }
        }

        public void AddUpdateLuceneIndex(Advertisement advertisement)
        {
            using (var writer = new IndexWriter(Directory, new IndexWriterConfig(AppLuceneVersion, Analyzer)))
            {
                var doc = new Document
                {
                    new StringField("ID", advertisement.ID.ToString(), Field.Store.YES),
                    new TextField("Title", advertisement.Title.ToLower(), Field.Store.YES),
                    new TextField("Description", advertisement.Description.ToLower(), Field.Store.YES)
                };

                writer.UpdateDocument(new Term("ID", advertisement.ID.ToString()), doc);
                writer.Flush(triggerMerge: false, applyAllDeletes: false);
            }
        }

        public IEnumerable<Advertisement> Search(string query)
        {
            // Открываем читатель индекса
            using (var reader = DirectoryReader.Open(Directory))
            {
                // Создаем поисковик
                var searcher = new IndexSearcher(reader);

                // Преобразуем запрос в нижний регистр для регистронезависимого сравнения
                query = query.ToLower();

                // Создаем запрос поиска с помощью WildcardQuery для учета опечаток в запросе
                var wildcardTerm = new Term("Title", "*" + query + "*");
                var wildcardQuery = new WildcardQuery(wildcardTerm);

                // Создаем запрос поиска с помощью FuzzyQuery для учета опечаток в запросе
                var fuzzyTerm = new Term("Title", query);
                var fuzzyQuery = new FuzzyQuery(fuzzyTerm);

                // Создаем объединенный запрос с помощью BooleanQuery
                var booleanQuery = new BooleanQuery();
                booleanQuery.Add(wildcardQuery, Occur.SHOULD); // Добавляем WildcardQuery с возможностью совпадения
                booleanQuery.Add(fuzzyQuery, Occur.SHOULD); // Добавляем FuzzyQuery с возможностью совпадения

                // Ищем совпадения
                var hits = searcher.Search(booleanQuery, 20).ScoreDocs;

                // Создаем список результатов
                var results = new List<Advertisement>();

                // Обходим найденные документы
                foreach (var hit in hits)
                {
                    // Получаем найденный документ
                    var foundDoc = searcher.Doc(hit.Doc);

                    // Создаем объект Advertisement и добавляем его в результаты
                    results.Add(new Advertisement
                    {
                        ID = Guid.Parse(foundDoc.Get("ID")),
                        Title = foundDoc.Get("Title"),
                        Description = foundDoc.Get("Description")
                    });
                }

                // Возвращаем результаты
                return results;
            }
        }

        public IEnumerable<string> AutoComplete(string prefix)
        {
            using (var reader = DirectoryReader.Open(Directory))
            {
                var searcher = new IndexSearcher(reader);

                prefix = prefix.ToLower();

                // Создаем запрос поиска с помощью WildcardQuery для учета опечаток в запросе
                var wildcardTerm = new Term("Title", "*" + prefix + "*");
                var wildcardQuery = new WildcardQuery(wildcardTerm);

                // Создаем запрос поиска с помощью FuzzyQuery для учета опечаток в запросе
                var fuzzyTerm = new Term("Title", prefix);
                var fuzzyQuery = new FuzzyQuery(fuzzyTerm);

                // Создаем объединенный запрос с помощью BooleanQuery
                var booleanQuery = new BooleanQuery();
                booleanQuery.Add(wildcardQuery, Occur.SHOULD); // Добавляем WildcardQuery с возможностью совпадения
                booleanQuery.Add(fuzzyQuery, Occur.SHOULD); // Добавляем FuzzyQuery с возможностью совпадения

                // Выполняем поиск с использованием префиксного запроса
                var hits = searcher.Search(booleanQuery, 10).ScoreDocs;

                // Собираем результаты поиска в список строк
                var suggestions = new List<string>();
                foreach (var hit in hits)
                {
                    var foundDoc = searcher.Doc(hit.Doc);
                    var suggestion = foundDoc.Get("Title");
                    suggestions.Add(suggestion);
                }

                return suggestions;
            }
        }



        public void Dispose()
        {
            Directory.Dispose();
        }
    }
}
