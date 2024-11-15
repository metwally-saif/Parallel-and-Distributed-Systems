## Program Structure

The program implements a parallelized version of the bubble sort algorithm. Key features include:

- **Parallel Odd-Even Transposition Sort:** Utilizes `Parallel.For` for concurrent processing.
- **Structured Design:** Handles the generation, sorting, and evaluation of data within a cohesive framework.
- **Evaluation:** Highlights the inherent limitations of bubble sort in a parallelized context through detailed performance testing.

---

## Parallelization Potential

The program adopts the **odd-even transposition sort**, a variation of bubble sort, to enable partial parallelization:

- **Odd and Even Phases:** Independent comparisons and swaps on disjoint pairs enable parallel execution.
- **Correctness Guaranteed:** No overlapping indices are accessed simultaneously during a phase.

---

## Partitioning of the Problem

The sorting process is divided into phases based on indices:

- **Even Phase:** Operates on pairs `(i, i+1)` where `i` is even.
- **Odd Phase:** Operates on pairs `(i, i+1)` where `i` is odd.

### Benefits of Partitioning:
- Minimizes index conflicts during processing.
- Requires global synchronization between phases for correctness.

---

## Evaluation of Key Factors

### Communication Needs
- Minimal communication during phases, as tasks operate on independent pairs.
- **Global synchronization** is necessary at the end of each phase.

### Data Dependencies
- Each phase is independent within itself but depends on the outcome of the previous phase.

### Synchronization Needs
- Global synchronization is implemented using `Parallel.For`, ensuring coherence when each loop ends.

### Load Balancing
- Evenly distributed workload based on indices.
- Slight imbalances may arise when array sizes are not divisible by thread counts.

---

## Performance Analysis

Performance testing results highlight the trade-offs in parallelizing bubble sort:

| Thread Count | Execution Time (ms) |
|--------------|---------------------|
| 2            | 127,147             |
| 3            | 215,623             |
| 4            | 255,352             |
| 6            | 261,877             |

### Observations:
1. **Optimal Thread Count:** Performance is best with 2 threads, as it minimizes parallelization overhead.
2. **Diminishing Returns:** Increasing threads beyond 2 results in:
   - Higher thread management overhead (synchronization, coordination).
   - Resource contention for shared resources (CPU cache, memory).
3. **Algorithmic Limitation:** Bubble sort's sequential nature limits scalability despite parallelization efforts.

---

## Conclusion

This project effectively demonstrates the potential and challenges of parallelizing bubble sort. Key takeaways include:

- **Bubble Sort Limitations:** High dependency on sequential operations makes bubble sort unsuitable for substantial parallelization gains.
- **Thread Count Trade-offs:** Increasing thread count can lead to diminishing returns due to overhead and contention.
- **Educational Value:** Despite limitations, this implementation serves as an insightful exploration of parallel programming techniques and their impact on performance.
