﻿#define PROCESS_TRACE
#undef PROCESS_TRACE

using System;
using Kernel.FOS_System.Collections;
using Kernel.Hardware;
using Kernel.Hardware.VirtMem;
using Kernel.FOS_System.IO;

namespace Kernel.Core.Processes
{
    [Compiler.PluggedClass]
    public unsafe class Process : FOS_System.Object
    {
        public List Threads = new List();
        public MemoryLayout TheMemoryLayout = new MemoryLayout();

        public uint Id;
        public FOS_System.String Name;

        public Scheduler.Priority Priority;

        private uint ThreadIdGenerator = 0;

        public Process(void* MainMethodPtr, uint AnId, FOS_System.String AName)
        {
#if PROCESS_TRACE
            BasicConsole.WriteLine(" > > Constructing process object...");
#endif
            Id = AnId;
            Name = AName;
            
#if PROCESS_TRACE
            BasicConsole.WriteLine(" > > Creating thread...");
#endif
            Thread mainThread = new Thread(MainMethodPtr, ThreadIdGenerator++);
#if PROCESS_TRACE
            BasicConsole.WriteLine(" > > Adding thread object...");
#endif
            Threads.Add(mainThread);
            
#if PROCESS_TRACE
            BasicConsole.WriteLine(" > > Setting up memory layout...");
#endif
            TheMemoryLayout.CR3 = GetCR3();
        }

        [Compiler.PluggedMethod(ASMFilePath=@"ASM\Processes\Process")]
        public static uint GetCR3()
        {
            return 0;
        }
    }
}
