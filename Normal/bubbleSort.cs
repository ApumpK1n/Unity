// 冒泡排序
// 基本思想：类似于水中冒泡，较大的数沉下去，较小的数慢慢冒起来。即较大的数慢慢往后排，较小的数慢慢往前排。
// 每一趟遍历，将一个最大的数移到序列末尾。
// 复杂度分析： 时间复杂度O(n^2) 空间复杂度O(1)

using System;
class Program
{
    static void Main(string[] args)
    {
        int[] arr = {2, 1, 3, 8, 5, 4, 9, 6};
        bubbleSort(arr);
        foreach(int i in arr)
        {
			Console.WriteLine(i);
		}
    }

    private static void bubbleSort(int[] arr){
        for (int i=0; i<arr.Length; i++) //遍历n趟
        {
            for(int j=0; j<arr.Length - 1 - i; j++) //遍历n趟，已经有n个数被排序，只需排序剩余数
            {
                if (arr[j] > arr[j+1]){
                    int temp = arr[j];
                    arr[j] = arr[j+1];
                    arr[j+1] = temp; 
                }
            }
        }
    }
}