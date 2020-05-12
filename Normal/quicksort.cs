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
        quickSort(arr, 0, arr.Length-1);
    }

    private static void quickSort(int[] arr, int left, int right){
        if (left >= right) return;
		int compare = arr[left];
        int low = left;
        int high = right;
        while(left < right){
            while(left < right && arr[right] >= compare){ // 从后往前找到一个小于compare的数
            	right--;
            }
            /*比key小的放左边*/
            arr[left] = arr[right];
        
            while(left < right && arr[left] <= compare){ // 从前往后找到一个大于compare的数
            	left ++;
            }
			/*比key大的放右边*/
            arr[right] = arr[left];
        }
		/*左边都比key小，右边都比key大。//将key放在游标当前位置。//此时low等于high */
        arr[left] = compare;

    	quickSort(arr, low, left-1);
    	quickSort(arr, left+1, high);
    }
}

 