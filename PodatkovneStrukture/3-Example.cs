using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PodatkovneStrukture
{
    public enum HanoiType
    {
        K13_01,
        K13_12,
        K13e_01,
        K13e_12,
        K13e_23,
        K13e_30,
        P4_01,
        P4_12,
        P4_23,
        P4_31,
        C4_01,
        C4_12,
        K4e_01,
        K4e_12,
        K4e_23,
    }

    public class Hanoi
    {
        private readonly int numDiscs;
        private readonly int numPegs;
        private readonly HanoiType type;

        private HashSet<long> setPrev;
        private HashSet<long> setCurrent;
        private List<long> setNew;
        private byte[] stateArray;
        private bool[] canMoveArray;

        private byte[] newState;
        private long currentState;
        private short currentDistance;

        public Hanoi(int numDiscs, int numPegs, HanoiType type)
        {
            this.numDiscs = numDiscs;
            this.numPegs = numPegs;
            this.type = type;
        }

        public static HanoiType SelectHanoiType()
        {
            Console.WriteLine(">> Select coloring type:");
            WriteHanoiTypes();
            return (HanoiType)Enum.Parse(typeof(HanoiType), Console.ReadLine());
        }

        private static void WriteHanoiTypes()
        {
            foreach (string s in Enum.GetNames(typeof(HanoiType)))
            {
                Console.WriteLine("\t" + (int)Enum.Parse(typeof(HanoiType), s) + " - " + s);
            }
        }

        long finalState = 0;

        /// <summary>
        /// Computes the length of a shortest path from the initial state to the final state. Only for small dimensions.
        /// </summary>
        public int ShortestPathForSmallDimension(out string path)
        {
            if (
                this.type != HanoiType.K13_01 && this.type != HanoiType.K13_12 &&
                this.type != HanoiType.K13e_01 && this.type != HanoiType.K13e_12 &&
                this.type != HanoiType.K13e_23 && this.type != HanoiType.K13e_30 &&
                this.type != HanoiType.K4e_01 && this.type != HanoiType.K4e_12 &&
                this.type != HanoiType.K4e_23 && this.type != HanoiType.C4_01 &&
                this.type != HanoiType.C4_12 && this.type != HanoiType.P4_01 &&
                this.type != HanoiType.P4_12 && this.type != HanoiType.P4_23 &&
                this.type != HanoiType.P4_31)
                throw new NotImplementedException("The search for this type is not implemented yet.");

            // For each disc we have its peg
            stateArray = new byte[this.numDiscs];
            canMoveArray = new bool[this.numPegs];

            setPrev = new HashSet<long>();
            setCurrent = new HashSet<long>();
            setNew = new List<long>();

            // Set initial and final states for each case
            {
                if (this.type == HanoiType.K13_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = FinalState();
                }
                else if (this.type == HanoiType.K13_12)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = FinalState();
                }
                else if (this.type == HanoiType.K13e_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                }
                else if (this.type == HanoiType.K13e_12)
                {
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                }
                else if (this.type == HanoiType.K13e_23)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                }
                else if (this.type == HanoiType.K13e_30)
                {
                    stateArray = ArrayAllEqual(3);
                    finalState = StateAllEqual(0);
                }
                else if (this.type == HanoiType.K4e_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                }
                else if (this.type == HanoiType.K4e_12)
                {
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                }
                else if (this.type == HanoiType.K4e_23)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                }
                else if (this.type == HanoiType.C4_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                }
                else if (this.type == HanoiType.C4_12)
                {
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                }
                else if (this.type == HanoiType.P4_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = StateAllEqual(1);
                }
                else if (this.type == HanoiType.P4_12)
                {
                    stateArray = ArrayAllEqual(1);
                    finalState = StateAllEqual(2);
                }
                else if (this.type == HanoiType.P4_23)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = StateAllEqual(3);
                }
                else if (this.type == HanoiType.P4_31)
                {
                    stateArray = ArrayAllEqual(3);
                    finalState = StateAllEqual(1);
                }
                else
                {
                    throw new Exception("Hanoi type state is not defined here!");
                }
            }

            currentDistance = 0;
            long initialState = StateToLong(stateArray);
            setCurrent.Add(initialState);

            path = "";

            long maxCardinality = 0;
            long maxMemory = 0;
            InitIgnoredStates(type);

            while (true) // Analiza posameznega koraka (i-tega premika)
            {
                if (maxCardinality < setCurrent.Count)
                    maxCardinality = setCurrent.Count;

                foreach (long num in setCurrent) // Znotraj i-tega premika preveri vsa možna stanja in se premakne v vse možne pozicije
                {
                    if (num == finalState)
                    {
                        return currentDistance;
                    }

                    byte[] tmpState = LongToState(num);
                    switch (type)
                    {
                        case HanoiType.K13_01:
                            MakeMoveForSmallDimension_K13_01_Fast(tmpState);
                            break;
                        case HanoiType.K13_12:
                            MakeMoveForSmallDimension_K13(tmpState);
                            break;
                        case HanoiType.K13e_01:
                        case HanoiType.K13e_12:
                        case HanoiType.K13e_23:
                        case HanoiType.K13e_30:
                            MakeMoveForSmallDimension_K13e(tmpState);
                            break;
                        case HanoiType.K4e_01:
                        case HanoiType.K4e_12:
                        case HanoiType.K4e_23:
                            MakeMoveForSmallDimension_K4e(tmpState);
                            break;
                        case HanoiType.C4_01:
                        case HanoiType.C4_12:
                            MakeMoveForSmallDimension_C4(tmpState);
                            break;
                        case HanoiType.P4_01:
                        case HanoiType.P4_12:
                        case HanoiType.P4_23:
                        case HanoiType.P4_31:
                            MakeMoveForSmallDimension_P4(tmpState);
                            break;
                    }
                }

                long mem = GC.GetTotalMemory(false);
                if (maxMemory < mem)
                {
                    maxMemory = mem;
                }

                // Ko se premaknemo iz vseh trenutnih stanj,
                // pregledamo nova trenutna stanja
                setPrev = setCurrent;
                setCurrent = new HashSet<long>();
                int elts = setNew.Count;
                for (int i = 0; i < elts; i++)
                {
                    setCurrent.Add(setNew[0]);
                    setNew.RemoveAt(0);
                }

                setNew = new List<long>();

                currentDistance++;

                Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                Console.CursorTop -= 2;
            }
        }

        private void InitIgnoredStates(HanoiType type)
        {
            switch (type)
            {
                case HanoiType.K13_01:
                    AddStateLeading3();
                    AddStateLeading1Then3();
                    break;
            }
        }

        private void AddStateLeading1Then3()
        {
            byte[] newState;
            for (int i = 1; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                newState[0] = 1;
                for (int j = 1; j <= i; j++)
                    newState[j] = 3;
            }
        }

        private void AddStateLeading3()
        {
            byte[] newState;
            for (int i = 0; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                for (int j = 0; j <= i; j++)
                    newState[j] = 3;
            }
        }

        private void AddNewState(byte[] state, int disc, byte toPeg)
        {
            newState = new byte[state.Length];
            for (int x = 0; x < state.Length; x++)
                newState[x] = state[x];
            newState[disc] = toPeg;
            currentState = StateToLong(newState);
            if (!setPrev.Contains(currentState))
            {
                setNew.Add(currentState);
            }
        }

        private void MakeMoveForSmallDimension_K13(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (canMoveArray[j])
                            {
                                AddNewState(state, i, j);
                            }
                        }
                    }
                    else // From other vertices we can only move to center
                    {
                        if (canMoveArray[0])
                        {
                            AddNewState(state, i, 0);
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
        }

        private void MakeMoveForSmallDimension_K13_01_Fast(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs - 2; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (canMoveArray[j])
                            {
                                AddNewState(state, i, j);
                            }
                        }
                    }
                    else // From other vertices we can only move to center
                    {
                        if (canMoveArray[0])
                        {
                            AddNewState(state, i, 0);
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
            // The second biggest:
            if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 0)
            {
                if (canMoveArray[0] && canMoveArray[2])
                {
                    AddNewState(state, numDiscs - 2, 2);
                }
                if (canMoveArray[0] && canMoveArray[3])
                {
                    AddNewState(state, numDiscs - 2, 3);
                }
                canMoveArray[0] = false;
            }
            else if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 1)
            {
                if (canMoveArray[0] && canMoveArray[1])
                {
                    AddNewState(state, numDiscs - 2, 1);
                }
                canMoveArray[0] = false;
            }
            else if (state[numDiscs - 2] > 1 && state[numDiscs - 1] == 1)
            {
                if (canMoveArray[state[numDiscs - 2]] && canMoveArray[0])
                {
                    AddNewState(state, numDiscs - 2, 0);
                }
                canMoveArray[state[numDiscs - 2]] = false;
            }
            // Biggest disk is moved only once
            if (state[numDiscs - 1] == 0)
            {
                if (canMoveArray[0] && canMoveArray[1])
                {
                    AddNewState(state, numDiscs - 1, 1);
                    //Console.WriteLine("The biggest is moved!\n");
                }
            }
        }

        private void MakeMoveForSmallDimension_K13e(byte[] state)
        {
            bool[] innerCanMoveArray = new bool[this.numPegs];
            ResetArray(innerCanMoveArray);
            byte[] innerNewState;

            for (int i = 0; i < numDiscs; i++)
            {
                if (innerCanMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        for (byte j = 1; j < numPegs; j++)
                        {
                            if (innerCanMoveArray[j])
                            {
                                innerNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    innerNewState[x] = state[x];
                                innerNewState[i] = j;
                                long innerCurrentState = StateToLong(innerNewState);
                                // Zaradi takih preverjanj potrebujemo hitro iskanje!
                                if (!setPrev.Contains(innerCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Add(innerCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        if (innerCanMoveArray[0])
                        {
                            innerNewState = new byte[state.Length];
                            for (int x = 0; x < state.Length; x++)
                                innerNewState[x] = state[x];
                            innerNewState[i] = 0;
                            long innerCurrentState = StateToLong(innerNewState);
                            if (!setPrev.Contains(innerCurrentState))
                            {
                                lock (setNew)
                                {
                                    setNew.Add(innerCurrentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 3 })
                        {
                            if (innerCanMoveArray[j])
                            {
                                innerNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    innerNewState[x] = state[x];
                                innerNewState[i] = j;
                                long innerCurrentState = StateToLong(innerNewState);
                                if (!setPrev.Contains(innerCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Add(innerCurrentState);
                                    }
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 2 })
                        {
                            if (innerCanMoveArray[j])
                            {
                                innerNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    innerNewState[x] = state[x];
                                innerNewState[i] = j;
                                long innerCurrentState = StateToLong(innerNewState);
                                if (!setPrev.Contains(innerCurrentState))
                                {
                                    lock (setNew)
                                    {
                                        setNew.Add(innerCurrentState);
                                    }
                                }
                            }
                        }
                    }
                }
                innerCanMoveArray[state[i]] = false;
            }
        }

        private void MakeMoveForSmallDimension_K4e(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 1, 2, 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 0, 2, 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
        }

        private void MakeMoveForSmallDimension_C4(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 2, 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 2, 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 1 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
        }

        private void MakeMoveForSmallDimension_P4(byte[] state)
        {
            ResetArray(canMoveArray);

            for (int i = 0; i < numDiscs; i++)
            {
                if (canMoveArray[state[i]])
                {
                    if (state[i] == 0)
                    {
                        foreach (byte j in new byte[] { 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 1)
                    {
                        foreach (byte j in new byte[] { 2 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 2)
                    {
                        foreach (byte j in new byte[] { 1, 3 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                    else if (state[i] == 3)
                    {
                        foreach (byte j in new byte[] { 0, 2 })
                        {
                            if (canMoveArray[j])
                            {
                                newState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    newState[x] = state[x];
                                newState[i] = j;
                                currentState = StateToLong(newState);
                                if (!setPrev.Contains(currentState))
                                {
                                    setNew.Add(currentState);
                                }
                            }
                        }
                    }
                }
                canMoveArray[state[i]] = false;
            }
        }

        private long StateToLong(byte[] state)
        {
            long num = 0;
            long factor = 1;
            for (int i = state.Length - 1; i >= 0; i--)
            {
                num += state[i] * factor;
                factor *= this.numPegs;
            }
            return num;
        }

        private long FinalState()
        {
            long num = 0;
            long factor = 1;
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                num += factor;
                factor *= this.numPegs;
            }
            return num;
        }

        private byte[] LongToState(long num)
        {
            byte[] tmpState = new byte[this.numDiscs];
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                tmpState[i] = (byte)(num % this.numPegs);
                num /= this.numPegs;
            }
            return tmpState;
        }

        private long StateAllEqual(int pegNumber)
        {
            long num = 0;
            long factor = 1;
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                num += pegNumber * factor;
                factor *= this.numPegs;
            }
            return num;
        }

        private byte[] ArrayAllEqual(byte pegNumber)
        {
            byte[] arr = new byte[this.numDiscs];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = pegNumber;
            return arr;
        }

        private void ResetArray(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = true;
        }
    }
}
