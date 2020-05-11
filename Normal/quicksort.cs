// 快速排序
/*
快速排序是分冶思想的应用，对冒泡排序的一种改进
基本思想：通过一趟排序将要排序的数据分割成独立的两部分，其中一部分的所有数据都比另外一部分的所有数据要小。
然后再对这两部分数据分别进行快速排序，整个排序过程递归进行

复杂度分析：O(nlogn) 空间复杂度O(1)
*/

using System;
class Program
{
    static void Main(string[] args)
    {
        int[] arr = {2, 1, 3, 8, 5, 4, 9, 6};
        quickSort(arr);
        foreach(int i in arr)
        {
			Console.WriteLine(i);
		}
    }

    private static void quickSort(int[] arr){
        
    }
}
