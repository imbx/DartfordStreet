using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Library : PuzzleBase {

    public List<Book> Books;

    void Update() {
        if(hasRequirement && GameController.current.database.GetProgressionState(reqID) && tag != "BasicInteraction") tag = "BasicInteraction";
        if(isInteractingThis){
            if(controller.isEscapePressed) {
                Debug.Log("[Library] Called Escape");
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
        bool isOrdered = true;

        Books.ForEach(x => {
            if(x.isImportant)
            {
                Debug.Log("[Library] Comparing " + idCounter + " to " + x.BookID);
                if(idCounter < x.BookID) idCounter = x.BookID;
                else isOrdered = false;
            }
        });

        if(isOrdered) this.OnEnd();
    }

    private int SearchBookPosition(Book book)
    {
        int bookPosition = 0;
        for(; bookPosition < Books.Count; bookPosition++)
        {
            if(Books[bookPosition].BookID == book.BookID) return bookPosition;
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