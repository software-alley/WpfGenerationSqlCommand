using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace WpfGenerationSqlCommand
{
    class VueModele
    {
        /// <summary>
        /// presenceTetiere = Test de l'existance de la tétière dans la base de données
        /// </summary>
        /// <param name="sTetiere">TETIERE A TESTER</param>
        /// <returns></returns>
        public bool presenceTetiere(string sTetiere)
        {
            bool bRetour = false;
            DB_CMCADataContext _DB = new DB_CMCADataContext();
            var query = from c in _DB.CHEVAUX
                        where c.ID_CHEVAL == sTetiere.Trim()
                        select c;
            // EXTRACTION DU NOMBRE D'OCCURRENCES
            if(query.Count()>0)
                bRetour = true; // La valeur existe dans la base de données
            return bRetour;
        }

        /// <summary>
        /// generateSQL = Génération du script SQL
        /// </summary>
        /// <param name="Lignes">Liste de string contenant les tétières à prendre en compte</param>
        /// <returns></returns>
        public string generateSQL(List <string>Lignes)
        {
            string prefixeSQL = "";
            string SQL = "";
            try
            {
                Dictionary<string, string> _ptr = this.lectureFichierXML("config_sql.xml");
                prefixeSQL = _ptr["PREFIXE"];
                SQL = "";

                foreach (string Row in Lignes)
                {
                    if (SQL == "")
                        SQL += String.Format(_ptr["FIRST_TOKEN"], Row);
                    else
                        SQL += String.Format(_ptr["TOKEN"], Row);
                }
                SQL += _ptr["SORTING"];
                SQL += _ptr["SUFFIXE"];
                System.Windows.Clipboard.SetText(prefixeSQL + SQL);
            }
            catch(Exception Erreur)
            {
                MessageBox.Show(Erreur.Message, Erreur.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return prefixeSQL+SQL;
        }

        public Dictionary <string, string> lectureFichierXML(string sFileName)
        {
            Dictionary<string, string> _ptr = new Dictionary<string, string>();  
            string fullFileName;

            fullFileName = System.IO.Directory.GetCurrentDirectory() +"\\" + sFileName.Trim();

            if(File.Exists(fullFileName)==true)
            {
                // LECTURE DU FICHIER XML
                XmlDocument xml = new XmlDocument();
                xml.Load(fullFileName);
                XmlNodeList nodes = xml.SelectNodes("//CONFIG_SQL"); 
                if(nodes.Count==1)
                {
                    foreach(XmlNode Cell in nodes[0].ChildNodes)
                    {
                        switch(Cell.Name)
                        {
                            case "PREFIXE": _ptr.Add("PREFIXE", Cell.InnerText); break;
                            case "FIRST_TOKEN": _ptr.Add("FIRST_TOKEN", Cell.InnerText); break;
                            case "TOKEN": _ptr.Add("TOKEN", Cell.InnerText); break;
                            case "SUFFIXE": _ptr.Add("SUFFIXE", Cell.InnerText); break;
                            case "SORTING": _ptr.Add("SORTING", Cell.InnerText); break;
                        }
                    }
                }
            }
            return _ptr;
        }
    }
}
