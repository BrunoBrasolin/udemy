//EXEMPLOS DE PARALLEL--------------------------------------------------------------------------------------------

//static void Write(char c)
//{
//    int i = 1000;
//    while (i -- > 0) Console.Write(c);
//}

//Task.Factory.StartNew(() => Write('.')); // Criando a task e iniciando


//Task t = new Task(() => Write('?')); // Apenas criando a task

//t.Start(); // Iniciando a task


//Write('-');



//PASSANDO UM OBJETO--------------------------------------------------------------------------------------------

//static void Write(object o)
//{
//    int i = 1000;
//    while (i-- > 0) Console.Write(o);
//}

//Task.Factory.StartNew(Write, 123); // Criando a task e iniciando


//Task t = new Task(Write, "hello"); // Apenas criando a task

//t.Start(); // Iniciando a task

//Write('-');



//RETORNANDO VALORES--------------------------------------------------------------------------------------------

//static int TextLength(object o)
//{
//    Console.Write($"\nTask with id: {Task.CurrentId} processing object {o}...");

//    Thread.Sleep(5000);

//    return o.ToString().Length;
//}

//string text1 = "first", text2 = "second";
//Task<int> task1 = new Task<int>(TextLength, text1);
//task1.Start();

//Task<int> task2 = Task.Factory.StartNew(TextLength, text2);

//Console.Write($"\nLength of '{text1}' is {task1.Result}"); // Mostra o resultado somente quando a função finalizar
//Console.Write($"\nLength of '{text2}' is {task2.Result}");



//CANCELANDO TASK COM TOKEN--------------------------------------------------------------------------------------------

//CancellationTokenSource cts = new CancellationTokenSource();
//CancellationToken token = cts.Token;

//token.Register(() => Console.WriteLine("Cancelattion requested"));

//Task infiniteTask = new Task(() =>
//{
//    int i = 0;
//    while (true)
//    {
//        token.ThrowIfCancellationRequested();
//        Console.WriteLine($"{i++}\t");
//    }
//}, token);

//infiniteTask.Start();

//Task.Factory.StartNew(() =>
//{
//    token.WaitHandle.WaitOne();
//    Console.WriteLine("Wait handle released, cancellation was requested");
//}, token);

//Console.ReadKey();
//cts.Cancel();



//CANCELANDO TASK COM VÍNCULO--------------------------------------------------------------------------------------------

//CancellationTokenSource planned = new CancellationTokenSource();
//CancellationTokenSource prevented = new CancellationTokenSource();
//CancellationTokenSource emergency = new CancellationTokenSource();

//CancellationTokenSource paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token, prevented.Token, emergency.Token); // Criado um vínculo entre os 3 tokens

//Task.Factory.StartNew(() =>
//{
//    int i = 0;
//    while (true)
//    {
//        paranoid.Token.ThrowIfCancellationRequested(); // Quando for solicitado o cancelamento de um dos 3 (planned, prevented ou emergency), a tarefa será cancelada
//        Console.WriteLine($"{i++}\t");

//        Thread.Sleep(500);
//    }
//}, paranoid.Token);

//Console.ReadKey();
//planned.Cancel();



//ESPERAR PARA CANCELAR--------------------------------------------------------------------------------------------


//CancellationTokenSource cts = new CancellationTokenSource();
//CancellationToken token = cts.Token;

//Task counter = new Task(() =>
//{
//    int i = 6;
//    while (i-- > 1)
//    {
//        Console.Write($"{i}\n");
//        Thread.Sleep(1000);
//    }
//});

//Task task = new Task(() =>
//{
//    //Thread.SpinWait(); //SpinWait.SpinUntil(); // Também para a thread, porém não libera o thread | Thread.Sleep() pausa a thread porém libera para outro processo
//    Console.WriteLine("Press any key to disarm; You have 5 seconds");
//    counter.Start();
//    bool cancelled = token.WaitHandle.WaitOne(5000); // Espera 5 segundos, retorna se o token foi cancelado ou nao

//    if (!cancelled)
//    {
//        Console.WriteLine("BOOOOOOOOOMMMM!!!");
//        cts.Cancel();
//    }
//    else Console.WriteLine("Bomb disarmed.");

//    token.ThrowIfCancellationRequested();
//}, token);
//task.Start();

//Console.ReadKey();
//cts.Cancel();



//ESPERAR TASK--------------------------------------------------------------------------------------------

//CancellationTokenSource cts = new CancellationTokenSource();
//CancellationToken token = cts.Token;

//Task task = new Task(() =>
//{
//    Console.WriteLine("I take 5 seconds");
//    for (int i = 0; i < 5; i++)
//    {
//        token.ThrowIfCancellationRequested();
//        Thread.Sleep(1000);
//    }

//    Console.WriteLine("I'm done");
//}, token);
//task.Start();

//Task task2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

////task.Wait(token); // Esperar somente uma tarefa finalizar

////Task.WaitAll(task, task2); // Espera todas as tarefas

////Task.WaitAny(task, task2); // Espera PELO MENOS UMA tarefa finalizar

////Task.WaitAll(new[] { task, task2 }, 4000); // Espera, NO MÁXIMO, 4 segundos

////Console.ReadKey();
////cts.Cancel();

////Task.WaitAll(new[] { task, task2 }, 4000, token); // Quando adicionado um token, invoca uma exception ao cancelar



//Task.WaitAll(new[] { task, task2 }, 4000, token);

//Console.WriteLine($"Task 1 status is {task.Status}");
//Console.WriteLine($"Task 2 status is {task2.Status}");



//EXCEPTION--------------------------------------------------------------------------------------------

//static void Test()
//{
//    Task t1 = Task.Factory.StartNew(() => { throw new InvalidOperationException("Can't do this") { Source = "t1" }; });

//    Task t2 = Task.Factory.StartNew(() => { throw new AccessViolationException("Can't access this") { Source = "t2" }; });

//    try
//    {
//        Task.WaitAll(t1, t2);
//    }
//    catch (AggregateException ex)
//    {
//        //foreach (Exception e in ex.InnerExceptions) Console.WriteLine($"Exception {e.GetType()} from {e.Source}: {e.Message}");
//        ex.Handle(e =>
//        {
//            if (e is InvalidOperationException)
//            {
//                Console.WriteLine("Invalid Operation!");
//                return true;
//            }

//            return false;
//        });
//    }
//}

//try
//{
//    Test();
//}
//catch (AggregateException ex)
//{
//    foreach (Exception e in ex.InnerExceptions)
//    {
//        Console.WriteLine($"Handled elsewhere: {e.GetType()}"); // Caso não tenha sido tratada no try catch da task, cairá aqui o restante
//    }
//}



//--------------------------------------------------------------------------------------------

Console.WriteLine("\nFinished.");
Console.ReadKey();