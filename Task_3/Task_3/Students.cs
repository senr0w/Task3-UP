//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Task_3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Students
    {
        public int StudID { get; set; }
        public string NameStud { get; set; }
        public string FamilyStud { get; set; }
        public int ObjectID { get; set; }
    
        public virtual Object Object { get; set; }
    }
}
