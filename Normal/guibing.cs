// 归并排序
// 基本思想：先拆分，再归并
// 复杂度分析： 时间复杂度O(nlog(n)) 空间复杂度O(n) 稳定算法

using System;
class Program
{
    static void Main(string[] args)
    {
        int[] arr = {2, 1, 3, 8, 5, 4, 9, 6};
        int[] res = new int[arr.Length];
        Guibing(arr, res, 0, arr.Length - 1);
        foreach(int i in arr)
        {
			Console.WriteLine(i);
		}
    }

    private static void Guibing(int[] arr, int[] res, int left, int right){
        if (left < right){
            int mid = left +(right - left) / 2;
            Guibing(arr, res, left, mid);
            Guibing(arr, res, mid + 1, right);
            Merge(arr, res, left, mid, right);
        }
    }

    private static void Merge(int[] arr, int[] res, int left, int mid, int right){
        int i = left;
        int j = mid + 1;
        for(int k = left; k<=right; k++){
            res[k] = arr[k]; // 保存起来
        }

        for(int k = left; k <= right; k++){
            if (i > mid) arr[k] = res[j++]; // 左半边用尽
            else if (j > right) arr[k] = res[i++]; //右半边用尽，取左半边
            else if (res[i] > res[j]) arr[k] = res[j++]; // 右半边小于左半边，取小的。
            else arr[k] = res[i++];
        }
    }
}