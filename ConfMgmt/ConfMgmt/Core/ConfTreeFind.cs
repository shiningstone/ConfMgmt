﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace JbConf
{
    public partial class ConfTree
    {
        private static Logger _log = new Logger("ConfTreeVisit");

        private class TagRecorder
        {
            public List<string> Tags = new List<string>();
            public List<string> Register(ConfItem item)
            {
                if (Tags.Contains(item.Tag))
                {
                    _log.Warn($"Tag conflict detected: Tag({item.Tag}) already exists in TagRecorder");
                }

                if (!string.IsNullOrEmpty(item.Tag))
                {
                    Tags.Add(item.Tag);
                }

                return Tags;
            }
            public List<string> Unregister(ConfItem item)
            {
                if (Tags.Contains(item.Tag))
                {
                    Tags.Remove(item.Tag);
                }

                return Tags;
            }
            public bool IsMatch(List<string> targetTags)
            {
                if (targetTags != null && targetTags.Count > 0)
                {
                    foreach (var tag in targetTags)
                    {
                        if (!Tags.Contains(tag))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        private TagRecorder RunningTag = new TagRecorder();

        private int _maxDepth;
        private int _depth = 0;
        public int MaxDepth
        {
            get
            {
                var str = ToString();
                return _maxDepth + 1;
            }
        }

        private bool VisitTree(string func, Func<ConfItem, int, bool> executor, ConfTree tree)
        {
            _log.Debug($@"Visit({func}) ConfTree: {tree.Path}/{tree.Name}({tree.Tag})");
            RunningTag.Register(tree);

            executor(tree, _depth);

            foreach (var c in tree.Sons)
            {
                _depth++;
                _maxDepth = _depth > _maxDepth ? _depth : _maxDepth;
                var ret = Visit(func, executor, c);
                _depth--;

                if (ret)
                {
                    RunningTag.Unregister(tree);
                    return true;
                }
            }

            RunningTag.Unregister(tree);

            return false;
        }
        public bool Visit(string func, Func<ConfItem, int, bool> executor, ConfItem item = null)
        {
            if (item == null)
            {
                _depth = 0;
                item = this;
            }

            var tree = item as ConfTree;
            if (tree != null)
            {
                var fastQuit = VisitTree(func, executor, tree);
                if (fastQuit)
                {
                    return true;
                }
            }
            else
            {
                _log.Debug($"Visit({func}) ConfItem: {item.Name}({item.Value})");
                if (executor(item, _depth))
                {
                    return true;
                }
            }

            return false;
        }

        public ConfItem Find(string target, List<string> tags = null)
        {
            ConfItem result = null;

            if (!target.Contains(@"/"))
            {
                Visit("Find", (item, level) =>
                {
                    if (item.Name == target && RunningTag.IsMatch(tags))
                    {
                        _log.Debug($"Find {item.Path}/{item.Name}");
                        result = item;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            else
            {
                var head_tail = ExtractHead(target);

                var tree = Find(head_tail[0], tags) as ConfTree;
                if (tree != null)
                {
                    var item = tree.Find(head_tail[1]);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return result;
        }
        public ConfItem FindStrict(string target, List<string> tags = null)
        {
            ConfItem result = null;

            if (!target.Contains(@"/"))
            {
                Visit("Find", (item, level) =>
                {
                    if (item.Name == target && RunningTag.IsMatch(tags))
                    {
                        _log.Debug($"Find {item.Path}/{item.Name}");
                        result = item;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            else
            {
                var head_tail = ExtractHead(target);

                var tree = Find(head_tail[0], tags) as ConfTree;
                if (tree != null)
                {
                    var item = tree.Find(head_tail[1]);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return result;
        }

        private static string[] ExtractHead(string path)
        {
            var strs = path.Split('/').ToList();
            return new string[] { strs[0], string.Join(@"/", strs.Skip(1).ToArray()) };
        }
    }
}
