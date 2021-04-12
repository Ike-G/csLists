using System;
using System.Linq;

namespace csLinkedLists
{
    class Program
    {
        static void Main(string[] args)
        {
            testList();
        }

        static void displayIntList(List<int> l) {
            for (int i = 0; i < l.Length; i++) {
                Console.WriteLine($"{i}: {l[i]}");
            }
        }

        static void testList() {
            // 1. Append 10 values as n^2 - Expectation [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
            // 2. Remove values 0, 2, 7, 4, and 5 - Expectation [1, 4, 16, 25, 49]
            // 3. Append 100 values as 2*n+1
            // 4. Set values 0, 5, 57, and 104 by key to 0
            ArrayList<int> testL = new ArrayList<int>(105);
            int[] removal = new int[] {0, 2, 7, 4, 5};

            Console.WriteLine("Test 1: ");
            for (int i = 0; i < 10; i++) {
                testL.append((int)Math.Pow(i,2));
            }
            Console.WriteLine("Test 2: ");
            for (int i = 0; i < 5; i++) {
                testL.pop(removal[i]);
            }
            Console.WriteLine("Test 3: ");
            for (int i = 0; i < 100; i++) {
                testL.append(2*i+1);
            }
            Console.WriteLine("Test 4: ");
            int[] arrayClone = testL.toArray();
            arrayClone[0] = 0;
            testL[0] = 0;
            arrayClone[5] = 0;
            testL[5] = 0; 
            arrayClone[57] = 0;
            testL[57] = 0;
            arrayClone[104] = 0;
            testL[104] = 0;
            bool failure = false;
            for (int i = 0; i < testL.Length; i++) {
                if (testL[i] != arrayClone[i]) { 
                    Console.WriteLine($"Failure at {i}");
                    failure = true;
                }
            }
            testL.display();
            if (!failure) 
                Console.WriteLine("Manipulation tests performed successfully.");
            else 
                Console.WriteLine("Tests failed.");

            failure = false;
            Random rnd = new Random();
            for (int i = 0; i < 20; i++) {
                int r = rnd.Next(1,80);
                testL.append(r);
            }
            Console.WriteLine("Finished appending random values.");
            testL.display();
            testL.sort();
            Console.WriteLine("Finished list sort.");
            testL.display();
            int lastVal = 0;
            for (int i = 0; i < testL.Length; i++) {
                if (lastVal > testL[i])
                    failure = true;
                lastVal = testL[i];
            }

            if (!failure) 
                Console.WriteLine("Sort tests performed successfully.");
            else 
                Console.WriteLine("Tests failed.");
        }
    }

    class List<T> {
        private Node startNode; 
        private Node endNode;
        private int _length;

        public int Length { get { return _length; } }

        public List() {
            startNode = new Node();
            _length = 0;  
        }

        public T this[int k] {
            get {
                Node current = startNode;
                for (int i = 0; i < k; i++) {
                    current = current.nextNode(); 
                }
                return current.getVal();
            }
            set { 
                Node current = startNode;
                for (int i = 0; i < k; i++) {
                    current = current.nextNode();
                }
                current.setVal(value);
            }
        }

        public void append(T value) {
            Node current = nthNode(_length);
            current.setVal(value);
            _length++;
        }

        public T pop(int k) {
            // Get the node prior to k that needs to have its pointer value remapped. If k == 0 then this is the startNode. 
            T val;
            if (k == 0) {
                val = startNode.getVal();
                startNode = nthNode(1);
            }
            else {
                Node node = nthNode(k-1);
                val = node.nextNode().getVal();
                node.setPointer(node.nextNode().nextNode());
            }
            _length--;
            return val;
        }

        private Node nthNode(int n) {
            Node current = startNode;
            for (int i = 0; i < n; i++) {
                current = current.nextNode();
            }
            return current;
        }

        public T[] toArray() {
            T[] newArray = new T[_length];
            for (int i = 0; i < _length; i++) {
                newArray[i] = this[i];
            }
            return newArray;
        }

        public void display() {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(string) 
                    || typeof(T) == typeof(char)) {
                for (int i = 0; i < _length; i++) {
                    Console.WriteLine($"{i}: {this[i]}");
                }
            }
        }

        class Node { 
            private Node _nextNode; 
            private T _val;
            private bool _hasVal; 

            public Node(T value) {
                _val = value;     
                _hasVal = true; 
                _nextNode = new Node();
            }

            public Node() {
                _hasVal = false;
            }

            public void setVal(T value) {
                _val = value;
                if (!_hasVal) 
                    _nextNode = new Node();
                _hasVal = true; 
            }

            public T getVal() { 
                if (hasVal())
                    return _val;
                throw new ArgumentOutOfRangeException();
            }

            public bool hasVal() {
                return _hasVal;
            }

            public Node nextNode() {
                return _nextNode;
            }

            public void setPointer(Node value) {
                _nextNode = value;
            }
        }

    }
    
    class ArrayList<T> {
        int maxSize;
        T[] values;
        int[] pointers; 
        bool[] unused;
        int startPointer; // Specifies the location of the start
        int endPointer; // Specifies the location of the end 
        int _length;

        public int Length { get { return _length; } }

        public T this[int k] {
            get { 
                if (k >= _length) 
                    throw new IndexOutOfRangeException();
                int current = startPointer; 
                for (int i = 0; i < k; i++) {
                    current = pointers[current];
                }
                return values[current];
            }
            set {  
                if (k >= _length) 
                    throw new IndexOutOfRangeException();
                int current = startPointer;
                for (int i = 0; i < k; i++) {
                    current = pointers[current];
                }
                values[current] = value;
            }
        }

        public ArrayList(int ms = 256) {
            maxSize = ms;
            values = new T[maxSize];
            pointers = Enumerable.Repeat(-1, maxSize).ToArray();
            unused = Enumerable.Repeat(true, maxSize).ToArray();
            startPointer = 0; 
            endPointer = 0;
            _length = 0;
        }

        public void append(T val) {
            int current = endPointer;
            try {
                while (!unused[current++]); 
            }
            catch (IndexOutOfRangeException) {
                current = 0;
                try {
                    while (!unused[current++] && current < pointers[endPointer]);
                } catch (IndexOutOfRangeException) {
                    Console.WriteLine("List out of space.");
                    return;
                }
            }
            values[--current] = val;
            unused[current] = false;
            pointers[endPointer] = current;
            endPointer = current;
            _length++;
        }

        public T pop(int key) {
            T val;
            int current = startPointer;
            if (key == 0) {
                val = values[current];
                unused[startPointer] = true;
                startPointer = pointers[startPointer];
                _length--; 
                return val;
            } else {
                try {
                    for (int i = 0; i < key-1; i++) {
                        current = pointers[current];
                    }
                } catch (IndexOutOfRangeException) {
                    throw new IndexOutOfRangeException($"Index {key} out of range.");
                }
                val = values[pointers[current]];
                unused[pointers[current]] = true;
                if (key == _length-1)
                    endPointer = current;
                else
                    pointers[current] = pointers[pointers[current]];
                _length--;
                return val;
            }
        }

        private void swap(int l, int h) {
            int current = startPointer;
            for (int i = 0; i < l-2; i++) {
                current = pointers[current];
            }
            int lp = current;
            for (int i = l; i < h; i++) {
                current = pointers[current];
            }
            int hp = current;
            int t = pointers[lp];
            pointers[lp] = pointers[hp];
            pointers[hp] = t;
        }
            
        public void sort() {
            if (typeof(T) == typeof(int)) {
                int[] stack = new int[_length]; // Stack for iterative implementation of commonly recursive algorithm
                int top = -1; // Pointer for stack.
                
                // Range of current partition
                int highBound = _length-1; 
                int lowBound = 0;

                stack[++top] = lowBound;
                stack[++top] = highBound;
                while (top >= 0) {
                    // Pop from stack, then decrement pointer.
                    highBound = stack[top--];
                    lowBound = stack[top--];

                    int x = (int)(object)this[highBound]; 
                    int j = (lowBound - 1); // Follow pointer
                    for (int i = lowBound; i <= highBound-1; i++) {
                        // While i <= x, increment j (which is always 1 less than i), then swap the values at j and i
                        // This does not get a perfect sort, however it is locally accurate.
                        if ((int)(object)this[i] <= x) {
                            j++;
                            swap(j, i);
                        }
                    }
                    swap(j+1, highBound);
                    int pivot = j+1;

                    if (pivot - 1 > lowBound) {
                        stack[++top] = lowBound;
                        stack[++top] = pivot - 1;
                    }

                    if (pivot + 1 < highBound) { 
                        stack[++top] = pivot + 1;
                        stack[++top] = highBound;
                    }
                }
            } else {
                throw new NotImplementedException();
            }
        }

        public T[] toArray() {
            int current = startPointer;
            T[] output = new T[_length];
            for (int i = 0; i < _length; i++) {
                output[i] = values[current];
                current = pointers[current];
            }
            return output;
        }

        public void display() {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(string) 
                    || typeof(T) == typeof(char)) {
                for (int i = 0; i < _length; i++) {
                    Console.WriteLine($"{i}: {this[i]}");
                }
            }
        }

        public void stateDump() {
            Console.WriteLine("values: ");
            for (int i = 0; i < maxSize; i++) {
                Console.WriteLine(values[i]);
            }
            Console.WriteLine("pointers: ");
            for (int i = 0; i < maxSize; i++) {
                Console.WriteLine(pointers[i]);
            }
            Console.WriteLine("unused: ");
            for (int i = 0; i < maxSize; i++) {
                Console.WriteLine(unused[i]);
            }
            Console.WriteLine($"Start pointer: {startPointer}");
            Console.WriteLine($"End pointer: {endPointer}");
            Console.WriteLine(_length);
        }
    }
}
