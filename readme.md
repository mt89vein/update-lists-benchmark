Update one list from another problem
----

Imaging that you have a certain list of elements in a database or just List<T> in memory.
And you have another - second list that needs to be applied changes to the first one.
If there are entries in both lists, then do an update.
If it is only in the old one, but not in the new one, then delete it, and if it is only in the new one, then add it.

The easiest way, just remove all the things from first and add all from second. Or just use second list :)

But not always you can use this approach. Such lists can be large enough, and you may want to do manual update only of some of them that was really changed.
Also, you may need to track or log added/deleted/updated for some reason, or mark as deleted, instead of real delete.
In such case you need algorithm, that can help you to find items that need add, delete, or update. And benchmark in this repository that covers different algorithms. 


I did two benchmarks.

[First one](LinqJoinBenchmark.JoinBenchmarks.SameLists.md), when all the list items is the same, but shuffled. Nothing to add or delete, just update one from other.
[Second one](LinqJoinBenchmark.JoinBenchmarks.50PercentMatch.md), when half of the list doesn't match, and both lists shuffled.