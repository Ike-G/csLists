using System;

namespace csLinkedLists
{
    class Program
    {
        static void Main(string[] args)
        {
            testList();
        }

        static void displayIntList(List<int> l) {
            for (int i = 0; i < l.Length(); i++) {
                Console.WriteLine($"{i}: {l[i]}");
            }
        }

        static void testList() {
            // 1. Append 10 values as n^2 - Expectation [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]
            // 2. Remove values 0, 2, 7, 4, and 5 - Expectation [1, 4, 16, 25, 49]
            // 3. Append 100 values as 2*n+1
            // 4. Set values 0, 5, 57, and 104 by key to 0
            List<int> testL = new List<int>();
            int[] removal = new int[] {0, 2, 7, 4, 5};

            Console.WriteLine("Test 1: ");
            for (int i = 0; i < 10; i++) {
                testL.append1((int)Math.Pow(i,2));
            }
            displayIntList(testL);

            Console.WriteLine("Test 2: ");
            for (int i = 0; i < 5; i++) {
                testL.remove(removal[i]);
            }
            displayIntList(testL);

            Console.WriteLine("Test 3: ");
            for (int i = 0; i < 100; i++) {
                testL.append1(2*i+1);
            }
            displayIntList(testL);

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
            for (int i = 0; i < testL.Length(); i++) {
                if (testL[i] != arrayClone[i]) 
                    Console.WriteLine($"Failure at {i}");
            }
            displayIntList(testL);
        }
    }

    class List<T> {
        private Node startNode; 
        private Node endNode;
        private int _length;

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

        // Method 1 - Recursively searches until end of list. Less data is stored in List, however it takes longer to modify.
        public void append1(T value) {
            Node current = nthNode(_length);
            current.setVal(value);
            _length++;
        }

        public void append2(T value) { 
            endNode.setVal(value); 
            endNode = endNode.nextNode();
            _length++;
        }

        public void remove(int k) {
            // Get the node prior to k that needs to have its pointer value remapped. If k == 0 then this is the startNode. 
            if (k == 0) 
                startNode = nthNode(1);
            else 
                nthNode(k-1).setPointer(nthNode(k+1));
            _length--;
        }

        public int Length() {
            return _length;
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

        public int binarySearch() {
            int[] arr = toArray();


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
        int maxSize = 256;
        T[] values;
        int[] pointers; 
        int startPointer; 

        public T this[int k] {
            get { 
                int current = startPointer; 
                for (int i = 0; i < k; i++) {
                    current = pointers[current];
                }
                return values[current];
            }
        }

        public ArrayList<T>() {
            values = new T[maxSize];
            pointers = new int[maxSize] {-1};
            startPointer = 0; 
        }


        public append(T value) {
            int current = startPointer;
            while (pointers[current] != -1) {
                current = pointers[current];
            }
            values[current] = value;
        }

        public remove(int key) {
            int current = startPointer;
            // Find the value at key-1
            // Find the value at key+1 
            // pointers[key-1] = key+1





    
}
