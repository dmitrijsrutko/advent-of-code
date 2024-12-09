using System;
using System.Collections.Generic;

namespace AoC_2024
{
	public class Day09
	{
        class BlockInfo
        {
            public long file_id;
            public long block_length;
            public long index;

            public BlockInfo(long file_id, long block_length, long index)
            {
                this.file_id = file_id;
                this.block_length = block_length;
                this.index = index;
            }
        }

        private static long GetCheckSum(long[] map)
        {
            long checksum = 0;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == -1) continue;
                checksum += map[i] * i;
            }

            return checksum;
        }

        private static long Solve01(long[] map)
        {
            // run the compaction
            int left = 0, right = map.Length -1;

            while (right > left)
            {
                while ((right > left) && (map[left] != -1))
                {
                    left++;
                }

                while ((right > left) && (map[right] == -1))
                {
                    right--;
                }

                if (right > left)
                {
                    map[left] = map[right];
                    map[right] = -1;
                    left++;
                    right--;
                }
            }

            return GetCheckSum(map);
        }

        private static void Compact(long[] map, BlockInfo file, List<BlockInfo> emptyBlocks)
        {
            for (int i = 0; i < emptyBlocks.Count; i++)
            {
                BlockInfo emptyBlock = emptyBlocks[i];
                if (emptyBlock.index >= file.index) break;
                if (emptyBlock.block_length >= file.block_length)
                {
                    for (int t = 0; t < file.block_length; t++)
                    {
                        map[emptyBlock.index + t] = file.file_id;
                        map[file.index + t] = -1;
                    }

                    emptyBlock.block_length -= file.block_length;
                    emptyBlock.index += file.block_length;

                    return;
                }
            }
        }

        private static long Solve02(long[] map, List<BlockInfo> files, List<BlockInfo> emptyBlocks)
        {
            // run file compaction
            for (int i = files.Count - 1; i >= 0; i--)
            {
                Compact(map, files[i], emptyBlocks);
            }

            return GetCheckSum(map);
        }

        public static void Run()
        {
            string day = "09";

            //StreamReader sr = new StreamReader("Day" + day + "/test" + day + ".txt");
            StreamReader sr = new StreamReader("Day" + day + "/data" + day + ".txt");

            string s = null;
            s = sr.ReadLine();
            sr.Close();

            // Pre-process
            List<long> map = new List<long>();
            List<BlockInfo> files = new List<BlockInfo>();
            List<BlockInfo> emptyBlocks = new List<BlockInfo>();

            for (int i = 0; i < s.Length; i++)
            {
                long block_length = s[i] - '0';
                long file_id = (i % 2 == 0) ? i / 2 : -1;

                BlockInfo blockInfo = new BlockInfo(file_id, block_length, map.Count);  // before updating map
                if (file_id != -1)
                {
                    files.Add(blockInfo);
                }
                else
                {
                    emptyBlocks.Add(blockInfo);
                }

                for (int t = 0; t < block_length; t++)
                {
                    map.Add(file_id);
                }
            }

            long started = Environment.TickCount;

            long solve01 = Solve01(map.ToArray());
            Console.WriteLine(solve01);

            long solve02 = Solve02(map.ToArray(), files, emptyBlocks);
            Console.WriteLine(solve02);

            long elapsed = Environment.TickCount - started;
            Console.WriteLine();
            Console.WriteLine("Elapsed: " + elapsed + " ms");
        }
    }
}

/*
 * 
6421128769094
6448168620520

Elapsed: 93 ms
*/
