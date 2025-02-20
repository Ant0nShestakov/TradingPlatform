﻿using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using AVS.Models.AdvertisementModels;
using Lucene.Net.Analysis;
using Microsoft.IdentityModel.Tokens;
using Lucene.Net.QueryParsers.Classic;

namespace AVS.Services
{
    public class LuceneIndex : IDisposable
    {
        private readonly LuceneVersion _appLuceneVersion = LuceneVersion.LUCENE_48;
        private readonly string _indexPath;
        private readonly Analyzer _analyzer;
        private readonly Lucene.Net.Store.Directory _directory;

        public LuceneIndex()
        {
            _indexPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LuceneIndex");
            _analyzer = new StandardAnalyzer(_appLuceneVersion);
            _directory = FSDirectory.Open(_indexPath);

            // Проверяем существование директории индекса
            if (!System.IO.Directory.Exists(_indexPath))
            {
                // Если не существует, создаем ее
                System.IO.Directory.CreateDirectory(_indexPath);
            }
        }

        public void AddUpdateLuceneIndex(Advertisement advertisement)
        {
            using (var writer = new IndexWriter(_directory, new IndexWriterConfig(_appLuceneVersion, _analyzer)))
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

        public List<Advertisement> Search(string query)
        {
            if (query.IsNullOrEmpty())
                return new List<Advertisement>();

            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);
                query = query.ToLower();
                var wildcardTerm = new Term("Title", "*" + query + "*");
                var wildcardQuery = new WildcardQuery(wildcardTerm);

                var fuzzyTerm = new Term("Title", query);
                var fuzzyQuery = new FuzzyQuery(fuzzyTerm);

                QueryParser parser = new QueryParser(LuceneVersion.LUCENE_48, "Title", _analyzer);
                Lucene.Net.Search.Query parsedQuery = parser.Parse(query.ToLower());

                var booleanQuery = new BooleanQuery();
                booleanQuery.Add(wildcardQuery, Occur.SHOULD);
                booleanQuery.Add(fuzzyQuery, Occur.SHOULD);
                booleanQuery.Add(parsedQuery, Occur.SHOULD);

                var hits = searcher.Search(booleanQuery, 10).ScoreDocs;
                var results = new List<Advertisement>();
                foreach (var hit in hits)
                {
                    var foundDoc = searcher.Doc(hit.Doc);
                    results.Add(new Advertisement
                    {
                        ID = Guid.Parse(foundDoc.Get("ID")),
                    });
                }
                return results;
            }
        }

        public IEnumerable<string> AutoComplete(string prefix)
        {
            using (var reader = DirectoryReader.Open(_directory))
            {
                var searcher = new IndexSearcher(reader);

                prefix = prefix.ToLower();

                var wildcardTerm = new Term("Title", "*" + prefix + "*");
                var wildcardQuery = new WildcardQuery(wildcardTerm);

                QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "Title", _analyzer);
                Lucene.Net.Search.Query parsedQuery = parser.Parse(prefix.ToLower());

                var fuzzyTerm = new Term("Title", prefix);
                var fuzzyQuery = new FuzzyQuery(fuzzyTerm);

                var booleanQuery = new BooleanQuery();
                booleanQuery.Add(wildcardQuery, Occur.SHOULD);
                booleanQuery.Add(fuzzyQuery, Occur.SHOULD);
                booleanQuery.Add(parsedQuery, Occur.SHOULD);
                var hits = searcher.Search(booleanQuery, 10).ScoreDocs;
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
            _directory.Dispose();
        }
    }
}
