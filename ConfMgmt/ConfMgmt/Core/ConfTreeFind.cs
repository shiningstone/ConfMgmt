using System;
using System.Collections.Generic;

namespace JbConf
{
    public partial class ConfTree
    {
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
                DebugDetail($@"Visit({func}) ConfTree: {tree.Path}/{tree.Name}({tree.Tag})");
                RunningTag.Register(tree);

                executor(item, _depth);

                foreach (var c in tree.Sons)
                {
                    _depth++;
                    var ret = Visit(func, executor, c);
                    _depth--;

                    if (ret)
                    {
                        RunningTag.Unregister(tree);
                        return true;
                    }
                }

                RunningTag.Unregister(tree);
            }
            else
            {
                DebugDetail($"Visit({func}) ConfItem: {item.Name}({item.Value})");
                if (executor(item, _depth))
                {
                    return true;
                }
            }

            return false;
        }
        public ConfItem Find(string itemName, List<string> tags = null)
        {
            ConfItem result = null;

            if (!itemName.Contains(@"/"))
            {
                Visit("Find", (item, level) =>
                {
                    if (item.Name == itemName && RunningTag.IsMatch(tags))
                    {
                        DebugDetail($"Find {item.Path}/{item.Name}");
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
                var head_tail = ExtractHead(itemName);

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
    }
}
