# Chinahoo.RedisLock
redis分布式锁
例子：
   var cli = new RedisClient("127.0.0.1:6379");
 cli.Set("StockCount", 10, 10);
            Parallel.For(0,500, (i) => { Task.Run(() => {
                System.Console.WriteLine(LockFactory.CreateLock(cli, "dabc", () =>
                {
                    int goodsCount = 1;
                    var stockCount = cli.Get<int>("StockCount");
                    if (stockCount > 0 && stockCount >= goodsCount)
                    {
                        stockCount -= goodsCount;
                        cli.Set("StockCount", stockCount, 10);
                        Console.WriteLine($"线程Id:{Thread.CurrentThread.ManagedThreadId}，抢购成功！库存数：{stockCount}");
                        return true;
                    }

                    Console.WriteLine($"线程Id:{Thread.CurrentThread.ManagedThreadId}，抢购失败！");
                    return false;
                }));
            }); });
            System.Console.ReadLine();
