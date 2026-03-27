# Design - Pseudo Code
# hash table operations

# insert(key, value)

FUNCTION insert(key, value)
    index = HashFunction(key) MOD tableSize
    current = buckets[index]
    
    WHILE current is not NULL
        IF current.key EQUALS key THEN
            current.value = value
            RETURN
        END IF
        current = current.next
    END WHILE
    
    newNode = CREATE new HashNode(key, value)
    newNode.next = buckets[index]
    buckets[index] = newNode
    count = count + 1
END FUNCTION


# search(key)

FUNCTION search(key)
    index = HashFunction(key) MOD tableSize
    current = buckets[index]
    
    WHILE current is not NULL
        IF current.key EQUALS key THEN
            RETURN current.value
        END IF
        current = current.next
    END WHILE
    
    RETURN null
END FUNCTION


# delete(key)

FUNCTION delete(key)
    index = HashFunction(key) MOD tableSize
    current = buckets[index]
    previous = NULL
    
    WHILE current is not NULL
        IF current.key EQUALS key THEN
            IF previous is NULL THEN
                buckets[index] = current.next
            ELSE
                previous.next = current.next
            END IF
            count = count - 1
            RETURN true
        END IF
        previous = current
        current = current.next
    END WHILE
    
    RETURN false
END FUNCTION


# linked list operations

# InsertAtHead(data)

FUNCTION InsertAtHead(data)
    newNode = CREATE new ListNode(data)
    newNode.next = head
    head = newNode
    count = count + 1
END FUNCTION


# InsertAtTail(data)

FUNCTION InsertAtTail(data)
    newNode = CREATE new ListNode(data)
    
    IF head is NULL THEN
        head = newNode
    ELSE
        current = head
        WHILE current.next is not NULL
            current = current.next
        END WHILE
        current.next = newNode
    END IF
    
    count = count + 1
END FUNCTION


# delete(condition)

FUNCTION delete(condition)
    IF head is NULL THEN
        RETURN false
    END IF
    
    IF condition(head.data) is true THEN
        head = head.next
        count = count - 1
        RETURN true
    END IF
    
    current = head
    WHILE current.next is not NULL
        IF condition(current.next.data) is true THEN
            current.next = current.next.next
            count = count - 1
            RETURN true
        END IF
        current = current.next
    END WHILE
    
    RETURN false
END FUNCTION


# time complexity

| Operation              | Average Case | Worst Case |
|------------------------|--------------|------------|
| Hash Table Insert      | O(1)         | O(n)       |
| Hash Table Search      | O(1)         | O(n)       |
| Hash Table Delete      | O(1)         | O(n)       |
| Linked List InsertHead | O(1)         | O(1)       |
| Linked List InsertTail | O(n)         | O(n)       |
| Linked List Search     | O(n)         | O(n)       |
| Linked List Delete     | O(n)         | O(n)       |