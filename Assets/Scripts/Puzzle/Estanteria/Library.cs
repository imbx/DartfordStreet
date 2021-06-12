using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Library : PuzzleBase {

    public List<Book> Books;

    public List<BookColor> ColorRowOrder;
    public List<int> Row2Order;
    public List<int> Row3Order;


    void Update() {
        if(hasRequirement && GameController.current.database.GetProgressionState(reqID) && tag != "BasicInteraction") tag = "BasicInteraction";
        if(isInteractingThis){
            if(controller.isEscapePressed) {
                Debug.Log("[Library] Called Escape");
                this.OnExit();
            }
        }
    }

    public void SwapBooks(Book book1, Book book2)
    {
        int bookPos1 = SearchBookPosition(book1);
        int bookPos2 = SearchBookPosition(book2);

        Books[bookPos1] = book2;
        Books[bookPos2] = book1;

        if(book1.isImportant || book2.isImportant) CheckBooks();
    }

    private void CheckBooks()
    {
        int idCounter = -1;

        int colorRowCounter = -1;
        int row2Counter = -1;
        int row3Counter = -1;

        bool isOrdered = true;

        foreach(Book x in Books)
        {
            if(x.Row == 0)
                {
                    if(x.bookColor == ColorRowOrder[colorRowCounter + 1])
                    {
                        Debug.Log("Color book right " + x.bookColor);
                        colorRowCounter++;
                    }  
                    else {
                        Debug.Log("Break because " + colorRowCounter);
                        isOrdered = false;
                        break;
                    }
                }

            else if(x.isImportant)
            {
                Debug.Log("[Library] Comparing " + idCounter + " to " + x.BookID + " from row " + x.Row);

                if(x.Row == 1)
                {
                    if(row2Counter == -1) row2Counter = x.BookID;

                    else if(x.BookID == row2Counter + 1)
                    {
                        Debug.Log("Row2 order right " + x.BookID);
                        row2Counter++;
                    }
                    else 
                    {
                        Debug.Log("Break because " + row2Counter + " != " + x.BookID);
                        isOrdered = false;
                        break;
                    }
                }

                if(x.Row == 2)
                {

                    if(row3Counter == -1) row3Counter = x.BookID;

                    else if(x.BookID == row3Counter + 1)
                    {
                        Debug.Log("Row3 order right " + x.BookID);
                        row3Counter++;
                    }
                    else 
                    {
                        Debug.Log("Break because " + row3Counter + " != " + x.BookID);
                        isOrdered = false;
                        break;
                    }
                }

                /*else if(x.strictOrder)
                {
                    if(idCounter + 1 == x.BookID) idCounter = x.BookID;
                    else 
                    {
                        isOrdered = false;
                        break;
                    }
                }
                else 
                {
                    if(idCounter < x.BookID) idCounter = x.BookID;
                    else 
                    {
                        isOrdered = false;
                        break;
                    }
                }*/
            }
        }

        if(isOrdered) this.OnEnd();
    }

    private int SearchBookPosition(Book book)
    {
        int bookPosition = 0;
        for(; bookPosition < Books.Count; bookPosition++)
        {
            if(Books[bookPosition].BookID == book.BookID && book.Row == Books[bookPosition].Row) return bookPosition;
        }
        return -1;
    }

    protected override void OnEnd(bool destroyGameObject = false)
    {
        BoxScripts.BoxUtils.SetTagRecursively(gameObject, "Untagged");
        Books.ForEach(x => Destroy(x));

        base.OnEnd(destroyGameObject);
    }
}