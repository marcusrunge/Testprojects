namespace Quantum.Bell
{
    open Microsoft.Quantum.Primitive;
    open Microsoft.Quantum.Canon;

	//Setzen des Zustandes des Qubits
    operation Set (desired: Result, qubit: Qubit) : ()
    {
        body
        {
			//Messen des Zustandes des aktuellen Qubits
            let current = M(qubit);
			//Wenn der aktuelle vom gewünschten Zustand abweicht, soll der aktuelle geändert(umgedreht) werden
            if (desired != current)
            {
				//Anwendung des Pauli-X Quantengatter
                X(qubit);
            }
        }
    }

	//Übergabe eines Zählers und Ergebnisses
	//Rückgabe zweier ganzzahliger Werte (Tupel)
	operation BellTest (count : Int, initial: Result) : (Int, Int, Int)
    {
        body
        {
			//Schaffung einer nach ihrer Schaffung veränderbaren Variable
            mutable numOnes = 0;
			mutable agree = 0;
            using (qubits = Qubit[2])
            {
				//"count" Schleifendurchläufe
                for (iteration in 1..count)
                {
					//Das Qubit 0 wird mit dem Anfangsergebniswert versehen
                    Set (initial, qubits[0]);
					//Das Qubit 1 wird mit dem Zero-Wert versehen
					Set (Zero, qubits[1]);
					//Anwendung des Pauli-X Quantengatter
					//X(qubits[0]);
					//Anwendung des Hadamard Quantengatter
					H(qubits[0]);
					//Anwendung des CNOT Quantengatter und Verschränkung von Qubit 0 und 1
					CNOT(qubits[0],qubits[1]);
					//Messung des Ergebnisses
                    let result = M(qubits[0]);
                    //Wenn die Qubits identisch sind, wird der Zähler erhöht
					if (M (qubits[1]) == result) 
                    {
                        set agree = agree + 1;
                    }
					//Wenn das Ergebnis "Eins" ist wird die veränderbare Variable um 1 erhöht
					if (result == One)
                    {
                        set numOnes = numOnes + 1;
                    }
                }
                Set(Zero, qubits[0]);
				Set(Zero, qubits[1]);
            }
			//Rückgabe wie oft |0> und |1> gemessen wurde
            return (count - numOnes, numOnes, agree);
        }
    }
}