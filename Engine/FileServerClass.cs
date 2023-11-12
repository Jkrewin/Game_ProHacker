using System;
using System.Collections.Generic;

namespace PH4_WPF.Engine
{
    /// <summary>
    /// Файл на сервере
    /// </summary>
    [Serializable]
    public class FileServerClass
    {
        private string fileName;
        private bool hasDel = false;

        /// <summary>
        /// Имя файла с перфексом
        /// </summary>
        public string FileName
        {
            get
            {
                if (Perfix == "") { return fileName; }
                else
                {
                    return fileName + "." + Perfix;
                }
            }
           // set => fileName = PatchToFileName(value); // убирает все после .
            set => fileName = value;
        }
        /// <summary>
        /// Обозначает перфикс файла тип файла exe, php итд
        /// </summary>
        public string Perfix { get; set; }
        /// <summary>
        /// Размер файла
        /// </summary>
        public float Size { get; set; }
        public ParameterClass FileСontents { get; set; }
        /// <summary>
        /// прова доступа к файлу
        /// </summary>
        public PremisionEnum Rights { get; set; }
        /// <summary>
        /// содержит файлы как каталог
        /// </summary>
        public List<FileServerClass> Dir;
        /// <summary>
        /// Это системный файл 
        /// </summary>
        public bool SystemFile;
        /// <summary>
        /// Файл удален (true)
        /// </summary>
        public bool HasDel { get => hasDel; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        [Serializable]
        public class ParameterClass
        {
            public string TextCommand;
            public TypeParam TypeInformation = TypeParam.file ;
            public int IntParam = 0;
            public byte ByteParam = 0;

            /// <summary>
            /// Тип запуска файла
            /// </summary>
            public enum TypeParam
            {
                exploit,
                shell,
                backdoor,
                file,
                goal_file,
                exe,
                dir,
                instructions
            }
        }

        /// <summary>
        /// Права доступа к файлам
        /// </summary>
        public enum PremisionEnum
        {
            none,
            OnlyAdmin,
            AdminAndUser,
            AdminUserGuest
        }

        /// <summary>
        /// Отображает список файлов без удаленных ранее
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static FileServerClass EnFileSys(FileServerClass dir, string[] p)
        {
            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == "") continue;
                dir = dir.Dir.Find(x => x.FileName == p[i]);
                if (dir == null) return null;
            }
            return (dir.HasDel == true) ? null : dir;
        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        public void FileDel() => hasDel = true;

        #region "Далее только статика"
        /// <summary>
        /// получить файл
        /// </summary>
        /// <param name="patch">Путь к файлу</param>
        /// <param name="srv">Сервер</param>
        /// <returns></returns>
        public static FileServerClass GetFile(string patch, Server srv)=> EnFileSys(srv.FileSys, patch.Split('/'));
        /// <summary>
        /// Существует ли эта директория
        /// </summary>
        /// <param name="patch">путь к директории</param>
        /// <param name="srv">сервер</param>
        /// <returns></returns>
        public static bool ExistDir(string patch, Server srv)
        {
            FileServerClass dir = EnFileSys(srv.FileSys, patch.Split('/'));
            return dir == null ? false : true;
        }
        /// <summary>
        /// Существует файл или нет 
        /// </summary>       
        public static bool Exist(string patch, string name, string perfix, Server srv)
        {
            FileServerClass dir = EnFileSys(srv.FileSys, patch.Split('/'));
            if (dir == null) { return false; } else {
                foreach (var item in dir.Dir )
                {
                    if (item.FileName == name + "." + perfix)
                    {
                        if (item.HasDel == true) return false; 
                        else return true;                       
                    }                    
                }
            }
            return false;
        }
        /// <summary>
        /// Существует файл или нет имя файла с перексом учитываеться
        /// </summary>       
        public static bool Exist(string patch, string nameDotperfix, Server srv)
        {
            FileServerClass dir = EnFileSys(srv.FileSys, patch.Split('/'));
            if (dir == null) { 
                return false; 
            }
            else
            {
                foreach (var item in dir.Dir)
                {
                    if (item.FileName == nameDotperfix)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Получить список файлов в папке 
        /// </summary>
        /// <param name="patch">Путь</param>
        /// <param name="srv">Сервер</param>
        /// <returns></returns>
        public static List<FileServerClass> GetInfoFiles(string patch, Server srv)
        {
            var tv = EnFileSys(srv.FileSys, patch.Split('/')).Dir;
            return (tv == null) ? new List<FileServerClass>() : tv.FindAll(x=> x.HasDel == false );
        }
        /// <summary>
        /// Удалить файл <b>Проверка на доступ не делаеться</b> 
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="srv"></param>
        /// <returns>true успешно удален если файл не системный или каталог с файлами </returns>
        public static bool DelFile(string patch, Server srv) {
            var tv = GetFile(patch,srv);
            if (tv.SystemFile == true) return false;
            if (tv.Dir.Count != 0) return false;          

            srv.FileSys.FileDel ();
            return false;
        }
        /// <summary>
        /// Если у вас права доступа к файлу
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="srv"></param>
        /// <returns>true- есть доступ</returns>
        public static bool CheckAccess(string patch, Server srv)
        {
            var tv = GetFile(patch, srv);
            int pr = 0;
            if (srv.Premision == Server.PremissionServerEnum.FullControl) { pr = 3; }
            else if (srv.Premision == Server.PremissionServerEnum.UserControl)            { pr = 2; }
            else if (srv.Premision == Server.PremissionServerEnum.GuestControl) { pr = 1; }

            int fl = 0;
            if (tv.Rights == PremisionEnum.OnlyAdmin) { fl = 3; }
            else if (tv.Rights == PremisionEnum.AdminAndUser) { fl = 2; }
            else if (tv.Rights == PremisionEnum.AdminUserGuest) { fl = 1; }

            return pr >= fl;
        }
        /// <summary>
        /// Получает из пути одно имя файла
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        public static string PatchToFileName(string patch) {
            string fileName = PatchToFileNamePerfix(patch);
            if (fileName.Contains('.'))
            {
                return fileName.Split ('.')[0];
            }
            else {
                return fileName;
            }
        }
        /// <summary>
        /// Получает из пути только перфикс
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        public static string PatchToPerfix(string patch) {            
            string fileName = PatchToFileNamePerfix(patch);
            if (fileName.Contains('.'))
            {
                return fileName.Split('.')[1];
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Получает из пути имя файла + перфикс
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        public static string PatchToFileNamePerfix(string patch)
        {
            string[] f = patch.Split('/');
            string fileName = f[^1];
            return fileName;
        }
        /// <summary>
        /// Только путь без имени файлы
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        public static string PatchOnly(string patch)
        {
            string[] f = patch.Split('/');
            string newpatch = "";
            for (int i = 0; i < f.Length-1; i++)
            {
                newpatch = newpatch + f[i] + "/";
            }
            return newpatch;
        }
        #endregion  

    }
}
