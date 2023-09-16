using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppNoteServer.Model
{
    public class NoteResponse
    {
        public List<Note> Notes { get; set; }

        public NoteResponse()
        {
            Notes = new List<Note>();
        }

    }
}
