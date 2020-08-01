
// 洗牌算法
using System;
class Program{

    static void Main(string[] args){

        int[] nums = new int[10]{1, 2, 3, 4,  5, 6, 7, 8, 9, 10};
        Shuffle(nums);
        for(int i=0; i<nums.Length; i++){
            Console.WriteLine(nums[i]);
        }
    }

    static void Shuffle(int[] shuffle){
        int n = shuffle.Length;
        for(int i=n-1; i>0; i--){
            Random random = new Random();
            int index = random.Next(0, i);
            int temp = shuffle[index];
            shuffle[index] = shuffle[i];
            shuffle[i] = temp;
        }
    }
}