using System;

class Program
{
    static void Main(string[] args)
    {
        // Single Dimentional Array
        Console.WriteLine("Implementation of Single Dimentional Array");
        int[] arr = new int[] {3,4,1,5,7,10,3,52};
        foreach(var num in arr){
            Console.Write(num+" ");
        }
        Console.WriteLine();

        int makrs_array_len = 5;
        int[] marks = new int[makrs_array_len];
        for(int i = 0; i < makrs_array_len; i++)
            marks[i] = i*2 + 1;
        for(int i = 0; i < makrs_array_len; i++)
        {
            Console.Write(marks[i]+" ");
        }

        // Multi Dimentional Array
        Console.WriteLine("\n\nImplementation of Multi Dimentional Array");
        // Building an 2D grid array
        int arm_size = 10;
        char[,] grid = new char[arm_size,arm_size];
        for(int row_idx = 0; row_idx < arm_size; row_idx++)
        {
            for(int col_idx = 0; col_idx < arm_size; col_idx++)
            {
                if(row_idx==col_idx || row_idx==arm_size-col_idx-1)
                    grid[row_idx,col_idx] = 'x';
                else grid[row_idx,col_idx] = '.';
            }
        }
        // Printing the 2D array
        for(int row_idx = 0; row_idx < arm_size; row_idx++)
        {
            for(int col_idx = 0; col_idx < arm_size; col_idx++)
            {
                Console.Write(grid[row_idx,col_idx]+" ");
            }
            Console.WriteLine();
        }

        // Jagged Array
        int row_size_of_ja = 4;
        int[][] project = new int[row_size_of_ja][];
        project[0] = new int[3];
        project[1] = new int[5];
        project[2] = new int[2];
        project[3] = new int[6];

        // Filling the jagged array with random numbers
        Console.WriteLine();
        Console.WriteLine("Jagged Array");
        Random rnd = new Random();
        for(int row_idx = 0; row_idx < project.Length; row_idx++)
        {
            for(int col_idx = 0; col_idx < project[row_idx].Length; col_idx++)
            {
                project[row_idx][col_idx] = rnd.Next(10);
            }
        }

        // Printing the jagged array
        for(int row_idx = 0; row_idx < project.Length; row_idx++)
        {
            for(int col_idx = 0; col_idx < project[row_idx].Length; col_idx++)
            {
                Console.Write(project[row_idx][col_idx] + " ");
            }
            Console.WriteLine();
        }
    }
}