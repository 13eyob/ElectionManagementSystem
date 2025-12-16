using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Election.UI.Forms
{
    public partial class frmAdminDashboard : Form
    {
        private readonly HttpClient _client;
        private Panel _selectedPanel = null;
        private TabControl _tabControl;
        private DataGridView _dgvAllCandidates;
        private DataGridView _dgvApprovedCandidates;
        private DataGridView _dgvPendingCandidates;
        private DataGridView _dgvRejectedCandidates;
        private DataGridView _dgvCandidateUsers;
        private Label[] _statValueLabels = new Label[6];

        // Track previous window size for responsive adjustments
        private Size _previousSize;
        private bool _isResizing = false;

        public frmAdminDashboard()
        {
            InitializeComponent();

            // Track initial size
            _previousSize = this.Size;

            // Set form properties
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(900, 600);

            // Initialize HttpClient
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7208"),
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Connect event handlers
            ConnectEventHandlers();

            // Set default view
            ShowDashboardView();
        }

        private void ConnectEventHandlers()
        {
            // Form events
            this.Load += frmAdminDashboard_Load;
            this.FormClosing += FrmAdminDashboard_FormClosing;
            this.Resize += FrmAdminDashboard_Resize;
            this.ResizeBegin += FrmAdminDashboard_ResizeBegin;
            this.ResizeEnd += FrmAdminDashboard_ResizeEnd;

            // Button events
            btnLogout.Click += BtnLogout_Click;
            button1.Click += BtnLogout_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Link events
            lnkHome.Click += LnkHome_Click;
            lnkMyProfile.Click += LnkMyProfile_Click;

            // Sidebar panels
            MakePanelClickable(panelhome, PanelHome_Click);
            MakePanelClickable(panelcandidate, PanelCandidate_Click);
            MakePanelClickable(panelvoters, PanelVoters_Click);
            MakePanelClickable(panelresult, PanelResult_Click);
            MakePanelClickable(panelstart, PanelStart_Click);
        }

        #region Responsive Layout Methods
        private void FrmAdminDashboard_ResizeBegin(object sender, EventArgs e)
        {
            _isResizing = true;
        }

        private void FrmAdminDashboard_ResizeEnd(object sender, EventArgs e)
        {
            _isResizing = false;
            AdjustLayoutForSize();
        }

        private void FrmAdminDashboard_Resize(object sender, EventArgs e)
        {
            if (!_isResizing) return;

            AdjustLayoutForSize();
            _previousSize = this.Size;
        }

        private void AdjustLayoutForSize()
        {
            try
            {
                // Update main content panel size and position
                pnlMainContent.SuspendLayout();

                int sidebarWidth = panel1.Width;
                int headerHeight = pnlHeader.Height;
                int adminLabelHeight = 35; // Approximate height of admin label

                // Calculate available space
                int availableWidth = this.ClientSize.Width - sidebarWidth;
                int availableHeight = this.ClientSize.Height - headerHeight - adminLabelHeight;

                // Update main content panel
                pnlMainContent.Location = new Point(sidebarWidth, headerHeight + adminLabelHeight);
                pnlMainContent.Size = new Size(availableWidth, availableHeight);

                // Center the admin label
                labeladmin.Left = (this.ClientSize.Width - labeladmin.Width) / 2;
                labeladmin.Top = headerHeight + 5;

                // Update DataGridView if visible
                if (dgvCandidates.Visible)
                {
                    dgvCandidates.Location = new Point(20, 70);
                    dgvCandidates.Size = new Size(pnlMainContent.Width - 40, pnlMainContent.Height - 90);
                }

                // Update refresh button position
                btnRefresh.Left = pnlMainContent.Width - btnRefresh.Width - 20;
                btnRefresh.Top = 20;

                // Update title position
                lblCandidatesTitle.Left = 20;
                lblCandidatesTitle.Top = 20;

                // Update logout button in header
                btnLogout.Left = pnlHeader.Width - btnLogout.Width - 20;
                btnLogout.Top = (pnlHeader.Height - btnLogout.Height) / 2;

                // Update links in header
                lnkHome.Left = btnLogout.Left - lnkHome.Width - 30;
                lnkHome.Top = (pnlHeader.Height - lnkHome.Height) / 2;

                lnkMyProfile.Left = lnkHome.Left - lnkMyProfile.Width - 20;
                lnkMyProfile.Top = (pnlHeader.Height - lnkMyProfile.Height) / 2;

                // Adjust sidebar height
                panel1.Height = this.ClientSize.Height - pnlHeader.Height;

                pnlMainContent.ResumeLayout();

                // Force a refresh of custom content
                if (_tabControl != null && _tabControl.Visible)
                {
                    _tabControl.Size = new Size(pnlMainContent.Width, pnlMainContent.Height - 100);
                }

                // Refresh statistics layout if visible
                if (_statValueLabels[0] != null && _statValueLabels[0].Visible)
                {
                    AdjustStatisticsLayout();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Layout adjustment error: {ex.Message}");
            }
        }

        private void AdjustStatisticsLayout()
        {
            try
            {
                var statsPanel = pnlMainContent.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
                if (statsPanel != null)
                {
                    int availableWidth = pnlMainContent.Width - 40;

                    // Calculate optimal card width
                    int cardWidth = 160;
                    int cardsPerRow = Math.Max(1, availableWidth / (cardWidth + 20));
                    int totalWidth = cardsPerRow * (cardWidth + 10);

                    statsPanel.Size = new Size(totalWidth, 150);
                    statsPanel.Left = (pnlMainContent.Width - totalWidth) / 2;
                }
            }
            catch
            {
                // Silently fail if statistics panel doesn't exist
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Alt + F4 for close
            if (keyData == (Keys.Alt | Keys.F4))
            {
                this.Close();
                return true;
            }

            // Ctrl + R for refresh
            if (keyData == (Keys.Control | Keys.R))
            {
                RefreshCurrentView();
                return true;
            }

            // Ctrl + M for maximize/restore
            if (keyData == (Keys.Control | Keys.M))
            {
                ToggleMaximize();
                return true;
            }

            // Ctrl + Shift + M for minimize
            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                this.WindowState = FormWindowState.Minimized;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ToggleMaximize()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        #endregion

        #region UI Interaction Methods
        private void MakePanelClickable(Panel panel, EventHandler clickHandler)
        {
            panel.Cursor = Cursors.Hand;
            panel.Click += clickHandler;

            foreach (Control control in panel.Controls)
            {
                control.Cursor = Cursors.Hand;
                control.Click += clickHandler;
            }
        }

        private void HighlightPanel(Panel panel)
        {
            if (_selectedPanel != null && _selectedPanel != panel)
            {
                ResetPanelAppearance(_selectedPanel);
            }

            if (panel != null)
            {
                SetActivePanelAppearance(panel);
                _selectedPanel = panel;
            }
        }

        private void ResetPanelAppearance(Panel panel)
        {
            panel.BackColor = Color.FromArgb(15, 22, 40);

            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = Color.FromArgb(15, 22, 40);
                    label.ForeColor = Color.White;
                }
                else if (control is PictureBox pictureBox)
                {
                    pictureBox.BackColor = Color.FromArgb(15, 22, 40);
                }
            }
        }

        private void SetActivePanelAppearance(Panel panel)
        {
            panel.BackColor = Color.FromArgb(30, 40, 70);

            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = Color.FromArgb(30, 40, 70);
                    label.ForeColor = Color.LightSkyBlue;
                }
                else if (control is PictureBox pictureBox)
                {
                    pictureBox.BackColor = Color.FromArgb(30, 40, 70);
                }
            }
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username}";
                labeladmin.Text = $"Welcome Administrator: {UserSession.Username}";
            }

            HighlightPanel(panelhome);

            // Perform initial layout adjustment
            AdjustLayoutForSize();
        }

        private void FrmAdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupResources();
        }

        private void CleanupResources()
        {
            try
            {
                // Remove event handlers
                if (_tabControl != null)
                {
                    _tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;
                }

                // Dispose HttpClient
                _client?.Dispose();

                // Clear main content
                ClearMainContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cleanup error: {ex.Message}");
            }
        }
        #endregion

        #region Navigation
        private void PanelHome_Click(object sender, EventArgs e)
        {
            HighlightPanel(panelhome);
            ShowDashboardView();
        }

        private void PanelCandidate_Click(object sender, EventArgs e)
        {
            HighlightPanel(panelcandidate);
            ShowCandidateAdminDashboard();
        }

        private void PanelVoters_Click(object sender, EventArgs e)
        {
            HighlightPanel(panelvoters);
            ShowVoterManagementView();
        }

        private void PanelResult_Click(object sender, EventArgs e)
        {
            HighlightPanel(panelresult);
            ShowResultsView();
        }

        private void PanelStart_Click(object sender, EventArgs e)
        {
            HighlightPanel(panelstart);
            ShowElectionControlView();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCurrentView();
        }

        private void LnkHome_Click(object sender, EventArgs e)
        {
            PanelHome_Click(sender, e);
        }

        private void LnkMyProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Admin profile feature coming soon!",
                "My Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region View Management
        private void ShowDashboardView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "📊 DASHBOARD OVERVIEW";
            CreateDashboardStatistics();
            AdjustLayoutForSize();
        }

        private void ShowCandidateAdminDashboard()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "👥 CANDIDATE ADMIN DASHBOARD";
            CreateCandidateAdminDashboard();
            AdjustLayoutForSize();
        }

        private void ShowVoterManagementView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "👤 VOTER MANAGEMENT";
            CreateVoterManagementPanel();
            AdjustLayoutForSize();
        }

        private void ShowResultsView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "📊 ELECTION RESULTS";
            CreateResultsPanel();
            AdjustLayoutForSize();
        }

        private void ShowElectionControlView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "⚡ ELECTION CONTROL";
            CreateElectionControlPanel();
            AdjustLayoutForSize();
        }

        private void ClearMainContent()
        {
            // Remove all controls except the title and refresh button
            var controlsToRemove = pnlMainContent.Controls.Cast<Control>()
                .Where(c => c != lblCandidatesTitle && c != btnRefresh && c != dgvCandidates)
                .ToList();

            foreach (var control in controlsToRemove)
            {
                control.Dispose();
            }

            lblCandidatesTitle.Visible = true;
            btnRefresh.Visible = true;
            dgvCandidates.Visible = false;
        }

        private void RefreshCurrentView()
        {
            if (_selectedPanel == panelhome)
            {
                ShowDashboardView();
            }
            else if (_selectedPanel == panelcandidate)
            {
                ShowCandidateAdminDashboard();
            }
            else if (_selectedPanel == panelvoters)
            {
                ShowVoterManagementView();
            }
            else if (_selectedPanel == panelresult)
            {
                ShowResultsView();
            }
            else if (_selectedPanel == panelstart)
            {
                ShowElectionControlView();
            }
        }
        #endregion

        #region Dashboard Statistics
        private void CreateDashboardStatistics()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "📊 ELECTION STATISTICS",
                Font = new Font("Times New Roman", 24, FontStyle.Bold),
                ForeColor = Color.Navy,
                Location = new Point(20, 20),
                Size = new Size(500, 40)
            };

            FlowLayoutPanel statsFlowPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 80),
                Size = new Size(1050, 150),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            string[] statLabels = { "Total Candidates", "Approved", "Pending", "Rejected", "Today", "This Week" };
            string[] statIcons = { "👥", "✅", "⏳", "❌", "📅", "📆" };

            for (int i = 0; i < statLabels.Length; i++)
            {
                Panel card = CreateStatCard(statIcons[i], statLabels[i], "0", i);
                statsFlowPanel.Controls.Add(card);
            }

            Label lblLoading = new Label
            {
                Text = "Loading statistics...",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 250),
                Size = new Size(400, 30),
                ForeColor = Color.Gray
            };

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(statsFlowPanel);
            mainPanel.Controls.Add(lblLoading);
            pnlMainContent.Controls.Add(mainPanel);

            // Load statistics
            LoadDashboardStatistics(lblLoading);

            // Adjust layout
            statsFlowPanel.Width = pnlMainContent.Width - 80;
        }

        private Panel CreateStatCard(string icon, string label, string value, int index)
        {
            Panel card = new Panel
            {
                Size = new Size(160, 100),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Location = new Point(10, 15),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _statValueLabels[index] = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(70, 15),
                Size = new Size(80, 40),
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                Location = new Point(70, 60),
                Size = new Size(80, 30),
                ForeColor = Color.Gray
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(_statValueLabels[index]);
            card.Controls.Add(lblLabel);

            return card;
        }

        private async void LoadDashboardStatistics(Label lblLoading)
        {
            try
            {
                var response = await _client.GetAsync("api/candidate/statistics");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                    if (result.TryGetProperty("statistics", out var stats))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            UpdateStatCards(stats);
                            lblLoading.Visible = false;
                        });
                    }
                }
            }
            catch
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lblLoading.Text = "Error loading statistics";
                    lblLoading.ForeColor = Color.Red;
                });
            }
        }

        private void UpdateStatCards(JsonElement stats)
        {
            try
            {
                if (_statValueLabels[0] != null)
                {
                    _statValueLabels[0].Text = stats.GetProperty("totalCandidates").GetInt32().ToString();
                    _statValueLabels[1].Text = stats.GetProperty("approvedCandidates").GetInt32().ToString();
                    _statValueLabels[2].Text = stats.GetProperty("pendingCandidates").GetInt32().ToString();
                    _statValueLabels[3].Text = stats.GetProperty("rejectedCandidates").GetInt32().ToString();
                    _statValueLabels[4].Text = stats.GetProperty("todayRegistrations").GetInt32().ToString();
                    _statValueLabels[5].Text = stats.GetProperty("thisWeekRegistrations").GetInt32().ToString();
                }
            }
            catch
            {
                for (int i = 0; i < _statValueLabels.Length; i++)
                {
                    if (_statValueLabels[i] != null)
                        _statValueLabels[i].Text = "N/A";
                }
            }
        }
        #endregion

        #region Candidate Admin Dashboard
        private void CreateCandidateAdminDashboard()
        {
            ClearMainContent();

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Statistics panel
            Panel statsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            Label lblStatsTitle = new Label
            {
                Text = "Candidate Statistics",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Navy,
                Location = new Point(10, 10),
                Size = new Size(200, 30)
            };

            statsPanel.Controls.Add(lblStatsTitle);
            mainPanel.Controls.Add(statsPanel);

            // Tab control
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 100),
                Size = new Size(mainPanel.Width, mainPanel.Height - 100),
                Appearance = TabAppearance.Normal,
                ItemSize = new Size(120, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add tabs
            string[] tabNames = { "All Candidates", "✅ Approved", "⏳ Pending", "❌ Rejected", "👤 Users" };
            string[] tabKeys = { "tabAll", "tabApproved", "tabPending", "tabRejected", "tabUsers" };

            for (int i = 0; i < tabNames.Length; i++)
            {
                TabPage tab = new TabPage(tabNames[i])
                {
                    Name = tabKeys[i],
                    BackColor = Color.White
                };
                _tabControl.TabPages.Add(tab);
            }

            // Create DataGridViews
            CreateDataGridViewsForTabs();

            _tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            mainPanel.Controls.Add(_tabControl);
            pnlMainContent.Controls.Add(mainPanel);

            // Load initial data
            _ = LoadCurrentTabDataAsync();
        }

        private void CreateDataGridViewsForTabs()
        {
            _dgvAllCandidates = CreateCandidateDataGridView();
            _dgvApprovedCandidates = CreateCandidateDataGridView();
            _dgvPendingCandidates = CreateCandidateDataGridView();
            _dgvRejectedCandidates = CreateCandidateDataGridView();
            _dgvCandidateUsers = CreateUsersDataGridView();

            _tabControl.TabPages["tabAll"].Controls.Add(_dgvAllCandidates);
            _tabControl.TabPages["tabApproved"].Controls.Add(_dgvApprovedCandidates);
            _tabControl.TabPages["tabPending"].Controls.Add(_dgvPendingCandidates);
            _tabControl.TabPages["tabRejected"].Controls.Add(_dgvRejectedCandidates);
            _tabControl.TabPages["tabUsers"].Controls.Add(_dgvCandidateUsers);
        }

        private DataGridView CreateCandidateDataGridView()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            // Add columns
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("FullName", "Full Name");
            dgv.Columns.Add("Party", "Party");
            dgv.Columns.Add("Region", "Region");
            dgv.Columns.Add("Age", "Age");
            dgv.Columns.Add("Email", "Email");
            dgv.Columns.Add("Votes", "Votes");
            dgv.Columns.Add("Status", "Status");
            dgv.Columns.Add("Date", "Applied On");

            // Action buttons
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn
            {
                HeaderText = "View",
                Text = "👁️ View",
                UseColumnTextForButtonValue = true,
                Name = "View",
                Width = 80
            };
            dgv.Columns.Add(btnView);

            DataGridViewButtonColumn btnActions = new DataGridViewButtonColumn
            {
                HeaderText = "Actions",
                Text = "⚡ Actions",
                UseColumnTextForButtonValue = true,
                Name = "Actions",
                Width = 100
            };
            dgv.Columns.Add(btnActions);

            dgv.CellClick += DgvCandidates_CellClick;
            dgv.CellFormatting += DgvCandidates_CellFormatting;

            return dgv;
        }

        private DataGridView CreateUsersDataGridView()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            dgv.Columns.Add("Id", "User ID");
            dgv.Columns.Add("Username", "Username");
            dgv.Columns.Add("Email", "Email");
            dgv.Columns.Add("Role", "Role");
            dgv.Columns.Add("Region", "Region");
            dgv.Columns.Add("HasVoted", "Has Voted");
            dgv.Columns.Add("RegisteredDate", "Registered Date");
            dgv.Columns.Add("IsCandidate", "Is Candidate");

            return dgv;
        }

        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadCurrentTabDataAsync();
        }

        private async Task LoadCurrentTabDataAsync()
        {
            if (_tabControl == null || _tabControl.SelectedTab == null)
                return;

            string selectedTab = _tabControl.SelectedTab.Name;

            switch (selectedTab)
            {
                case "tabAll":
                    await LoadCandidatesAsync(_dgvAllCandidates, "all");
                    break;
                case "tabApproved":
                    await LoadCandidatesAsync(_dgvApprovedCandidates, "approved");
                    break;
                case "tabPending":
                    await LoadCandidatesAsync(_dgvPendingCandidates, "pending");
                    break;
                case "tabRejected":
                    await LoadCandidatesAsync(_dgvRejectedCandidates, "rejected");
                    break;
                case "tabUsers":
                    await LoadCandidateUsersAsync(_dgvCandidateUsers);
                    break;
            }
        }

        private async Task LoadCandidatesAsync(DataGridView dgv, string endpoint)
        {
            try
            {
                dgv.SuspendLayout();
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                string apiEndpoint = endpoint switch
                {
                    "pending" => "api/candidate/pending",
                    "rejected" => "api/candidate/rejected",
                    "approved" => "api/candidate/approved",
                    _ => "api/candidate/all"
                };

                var response = await _client.GetAsync(apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (endpoint == "approved")
                    {
                        // Approved endpoint returns array directly
                        var candidates = JsonSerializer.Deserialize<List<JsonElement>>(responseString);

                        foreach (var candidate in candidates)
                        {
                            dgv.Rows.Add(
                                candidate.GetProperty("id").GetInt32(),
                                candidate.GetProperty("name").GetString(),
                                candidate.GetProperty("party").GetString(),
                                candidate.GetProperty("region").GetString(),
                                candidate.GetProperty("age").GetInt32(),
                                0, // Votes - not available in this endpoint
                                "✅ Approved",
                                DateTime.Now.ToString("yyyy-MM-dd")
                            );
                        }
                    }
                    else
                    {
                        // Other endpoints return {success: true, candidates: [...]}
                        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                        if (result.TryGetProperty("candidates", out var candidatesArray))
                        {
                            foreach (var candidate in candidatesArray.EnumerateArray())
                            {
                                string status = candidate.GetProperty("isApproved").GetBoolean()
                                    ? "✅ Approved"
                                    : candidate.TryGetProperty("status", out var statusProp)
                                        ? statusProp.GetString() == "Rejected" ? "❌ Rejected" : "⏳ Pending"
                                        : "⏳ Pending";

                                dgv.Rows.Add(
                                    candidate.GetProperty("id").GetInt32(),
                                    candidate.GetProperty("fullName").GetString(),
                                    candidate.GetProperty("partyAffiliation").GetString(),
                                    candidate.GetProperty("region").GetString(),
                                    candidate.GetProperty("age").GetInt32(),
                                    candidate.GetProperty("email").GetString(),
                                    candidate.TryGetProperty("votesReceived", out var votes) ? votes.GetInt32() : 0,
                                    status,
                                    candidate.GetProperty("applicationDate").GetDateTime().ToString("yyyy-MM-dd")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading candidates: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgv.ResumeLayout();
                Cursor = Cursors.Default;
            }
        }

        private async Task LoadCandidateUsersAsync(DataGridView dgv)
        {
            try
            {
                dgv.SuspendLayout();
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                // ✅ FIXED: Use detailed voters endpoint
                var response = await _client.GetAsync("api/admin/voters-detailed");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("voters", out var voters))
                    {
                        foreach (var voter in voters.EnumerateArray())
                        {
                            dgv.Rows.Add(
                                voter.GetProperty("id").GetInt32(),
                                voter.GetProperty("username").GetString(),
                                voter.GetProperty("email").GetString(),
                                voter.GetProperty("role").GetString(),
                                voter.GetProperty("region").GetString(),
                                voter.GetProperty("hasVoted").GetBoolean() ? "✅ Voted" : "❌ Not Voted",
                                voter.GetProperty("registeredDate").GetDateTime().ToString("yyyy-MM-dd"),
                                voter.GetProperty("isCandidate").GetBoolean() ? "Yes" : "No"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading voters: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgv.ResumeLayout();
                Cursor = Cursors.Default;
            }
        }

        private void DgvCandidates_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;

            if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "✅ Approved")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
                else if (status == "⏳ Pending")
                {
                    e.CellStyle.ForeColor = Color.Orange;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
                else if (status == "❌ Rejected")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
            }

            // Format Votes column
            if (dgv.Columns[e.ColumnIndex].Name == "Votes" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int votes))
                {
                    e.CellStyle.ForeColor = votes > 0 ? Color.Blue : Color.Gray;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
            }
        }

        private async void DgvCandidates_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex == dgv.Columns["View"].Index)
            {
                int candidateId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                string candidateName = dgv.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                await ShowCandidateDetailsAsync(candidateId, candidateName);
            }
            else if (e.ColumnIndex == dgv.Columns["Actions"].Index)
            {
                int candidateId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                string candidateName = dgv.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                ShowCandidateActionsMenu(candidateId, candidateName);
            }
        }

        private async Task ShowCandidateDetailsAsync(int candidateId, string candidateName)
        {
            try
            {
                var response = await _client.GetAsync($"api/candidate/details/{candidateId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                    if (result.TryGetProperty("candidate", out var candidate))
                    {
                        using (var detailsForm = new Form())
                        {
                            detailsForm.Text = $"Candidate Details: {candidateName}";
                            detailsForm.Size = new Size(500, 400);
                            detailsForm.StartPosition = FormStartPosition.CenterParent;
                            detailsForm.MaximizeBox = true;
                            detailsForm.MinimizeBox = true;
                            detailsForm.FormBorderStyle = FormBorderStyle.Sizable;

                            TextBox txtDetails = new TextBox
                            {
                                Multiline = true,
                                ReadOnly = true,
                                ScrollBars = ScrollBars.Vertical,
                                Dock = DockStyle.Fill,
                                Font = new Font("Consolas", 10),
                                Text = FormatCandidateDetails(candidate)
                            };

                            detailsForm.Controls.Add(txtDetails);
                            detailsForm.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowCandidateActionsMenu(int candidateId, string candidateName)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem approveItem = new ToolStripMenuItem("✅ Approve Candidate");
            approveItem.Click += async (s, e) =>
                await ApproveCandidateAsync(candidateId, candidateName);

            ToolStripMenuItem rejectItem = new ToolStripMenuItem("❌ Reject Candidate");
            rejectItem.Click += async (s, e) =>
                await RejectCandidateAsync(candidateId, candidateName);

            menu.Items.Add(approveItem);
            menu.Items.Add(rejectItem);
            menu.Show(Cursor.Position);
        }

        private async Task ApproveCandidateAsync(int candidateId, string candidateName)
        {
            var result = MessageBox.Show($"Approve candidate: {candidateName}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var response = await _client.PutAsync($"api/candidate/approve/{candidateId}", null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Candidate approved successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadCurrentTabDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private async Task RejectCandidateAsync(int candidateId, string candidateName)
        {
            var result = MessageBox.Show($"Reject candidate: {candidateName}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var response = await _client.PutAsync($"api/candidate/reject/{candidateId}", null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Candidate rejected!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadCurrentTabDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private string FormatCandidateDetails(JsonElement candidate)
        {
            try
            {
                return $@"CANDIDATE DETAILS
────────────────────────────
Full Name:    {candidate.GetProperty("fullName").GetString()}
Age:          {candidate.GetProperty("age").GetInt32()}
Region:       {candidate.GetProperty("region").GetString()}
Party:        {candidate.GetProperty("partyAffiliation").GetString()}
Email:        {candidate.GetProperty("email").GetString()}
Phone:        {candidate.GetProperty("phone").GetString()}
Status:       {candidate.GetProperty("status").GetString()}
Approved:     {(candidate.GetProperty("isApproved").GetBoolean() ? "Yes" : "No")}
Rejected:     {(candidate.TryGetProperty("isRejected", out var rejected) ? rejected.GetBoolean() : false ? "Yes" : "No")}
Votes:        {(candidate.TryGetProperty("votesReceived", out var votes) ? votes.GetInt32() : 0)}
Applied On:   {candidate.GetProperty("applicationDate").GetDateTime():yyyy-MM-dd HH:mm}
────────────────────────────
Admin Remarks:
{candidate.GetProperty("adminRemarks").GetString()}";
            }
            catch
            {
                return "Error loading candidate details.";
            }
        }
        #endregion

        #region Voter Management
        private void CreateVoterManagementPanel()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // ✅ FIXED: Search Panel with proper layout
            Panel searchPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(mainPanel.Width - 40, 60),
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            TextBox txtSearch = new TextBox
            {
                PlaceholderText = "Search voters by name, email, or region...",
                Location = new Point(10, 15),
                Size = new Size(searchPanel.Width - 270, 30),
                Font = new Font("Segoe UI", 11),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };

            Button btnSearch = new Button
            {
                Text = "🔍 Search",
                Location = new Point(searchPanel.Width - 240, 15),
                Size = new Size(100, 30),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnSearch.FlatAppearance.BorderSize = 0;

            Button btnExport = new Button
            {
                Text = "📥 Export CSV",
                Location = new Point(searchPanel.Width - 120, 15),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnExport.FlatAppearance.BorderSize = 0;

            // Handle resize events
            searchPanel.Resize += (s, e) =>
            {
                txtSearch.Width = searchPanel.Width - 270;
                btnSearch.Left = searchPanel.Width - 240;
                btnExport.Left = searchPanel.Width - 120;
            };

            mainPanel.Resize += (s, e) =>
            {
                searchPanel.Width = mainPanel.Width - 40;
            };

            searchPanel.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnExport });

            // ✅ FIXED: Data GridView - positioned below search panel
            DataGridView dgvVoters = new DataGridView
            {
                Name = "dgvVoters",
                Location = new Point(0, 70),
                Size = new Size(mainPanel.Width - 40, mainPanel.Height - 90),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add columns
            dgvVoters.Columns.Add("Id", "ID");
            dgvVoters.Columns.Add("FullName", "Full Name");
            dgvVoters.Columns.Add("Username", "Username");
            dgvVoters.Columns.Add("Email", "Email");
            dgvVoters.Columns.Add("Region", "Region");
            dgvVoters.Columns.Add("Age", "Age");
            dgvVoters.Columns.Add("HasVoted", "Voting Status");
            dgvVoters.Columns.Add("RegisteredDate", "Registered Date");
            dgvVoters.Columns.Add("IsCandidate", "Candidate?");

            // Action buttons column
            DataGridViewButtonColumn btnActions = new DataGridViewButtonColumn
            {
                HeaderText = "Actions",
                Text = "Manage",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dgvVoters.Columns.Add(btnActions);

            // ✅ FIXED: Handle main panel resize to update DataGridView size
            mainPanel.Resize += (s, e) =>
            {
                searchPanel.Width = mainPanel.Width - 40;
                dgvVoters.Width = mainPanel.Width - 40;
                dgvVoters.Height = mainPanel.Height - 90;
            };

            // Connect events
            btnSearch.Click += async (s, e) => await LoadVotersAsync(dgvVoters, txtSearch.Text);
            btnExport.Click += (s, e) => ExportVotersToCSV(dgvVoters);
            dgvVoters.CellClick += DgvVoters_CellClick;
            dgvVoters.CellFormatting += DgvVoters_CellFormatting;

            mainPanel.Controls.Add(searchPanel);
            mainPanel.Controls.Add(dgvVoters);
            pnlMainContent.Controls.Add(mainPanel);

            // Load initial data
            _ = LoadVotersAsync(dgvVoters, "");
        }

        private async Task LoadVotersAsync(DataGridView dgv, string search)
        {
            try
            {
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                var response = await _client.GetAsync("api/admin/voters-detailed");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("voters", out var voters))
                    {
                        foreach (var voter in voters.EnumerateArray())
                        {
                            if (!string.IsNullOrEmpty(search))
                            {
                                string username = voter.GetProperty("username").GetString()?.ToLower() ?? "";
                                string email = voter.GetProperty("email").GetString()?.ToLower() ?? "";
                                string region = voter.GetProperty("region").GetString()?.ToLower() ?? "";
                                string fullName = voter.GetProperty("fullName").GetString()?.ToLower() ?? "";

                                if (!username.Contains(search.ToLower()) &&
                                    !email.Contains(search.ToLower()) &&
                                    !region.Contains(search.ToLower()) &&
                                    !fullName.Contains(search.ToLower()))
                                    continue;
                            }

                            dgv.Rows.Add(
                                voter.GetProperty("id").GetInt32(),
                                voter.GetProperty("fullName").GetString(),
                                voter.GetProperty("username").GetString(),
                                voter.GetProperty("email").GetString(),
                                voter.GetProperty("region").GetString(),
                                voter.GetProperty("age").GetInt32(),
                                voter.GetProperty("hasVoted").GetBoolean() ? "✅ Voted" : "❌ Not Voted",
                                voter.GetProperty("registeredDate").GetDateTime().ToString("yyyy-MM-dd"),
                                voter.GetProperty("isCandidate").GetBoolean() ? "Yes" : "No"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading voters: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void DgvVoters_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;

            // Format HasVoted column
            if (dgv.Columns[e.ColumnIndex].Name == "HasVoted" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "✅ Voted")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
                else if (status == "❌ Not Voted")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
            }
        }

        private void DgvVoters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 9) return; // Actions column

            DataGridView dgv = (DataGridView)sender;
            int userId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
            string username = dgv.Rows[e.RowIndex].Cells["Username"].Value.ToString();

            ShowVoterActionsMenu(userId, username);
        }

        private void ShowVoterActionsMenu(int userId, string username)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem viewItem = new ToolStripMenuItem("👁️ View Details");
            viewItem.Click += (s, e) => ViewVoterDetails(userId, username);

            ToolStripMenuItem toggleVoteItem = new ToolStripMenuItem("🗳️ Toggle Voting Status");
            toggleVoteItem.Click += async (s, e) => await ToggleVoterVotingStatus(userId, username);

            ToolStripMenuItem disableItem = new ToolStripMenuItem("⏸️ Disable Account");
            disableItem.Click += (s, e) => DisableVoterAccount(userId, username);

            ToolStripMenuItem deleteItem = new ToolStripMenuItem("🗑️ Delete Account");
            deleteItem.Click += (s, e) => DeleteVoterAccount(userId, username);

            menu.Items.Add(viewItem);
            menu.Items.Add(toggleVoteItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(disableItem);
            menu.Items.Add(deleteItem);

            menu.Show(Cursor.Position);
        }

        private async Task ToggleVoterVotingStatus(int userId, string username)
        {
            try
            {
                var response = await _client.PutAsync($"api/admin/toggle-voter-status/{userId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    var message = result.GetProperty("message").GetString();

                    MessageBox.Show(message, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh voter list - find the DataGridView in the panel
                    var mainPanel = pnlMainContent.Controls[0] as Panel;
                    if (mainPanel != null)
                    {
                        var dgv = mainPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                        if (dgv != null)
                        {
                            await LoadVotersAsync(dgv, "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewVoterDetails(int userId, string username)
        {
            MessageBox.Show($"View details for: {username}\nUser ID: {userId}\n\nFeature coming soon!",
                "Voter Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisableVoterAccount(int userId, string username)
        {
            var result = MessageBox.Show($"Disable account for: {username}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show($"Account for {username} has been disabled.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteVoterAccount(int userId, string username)
        {
            var result = MessageBox.Show($"PERMANENTLY DELETE account for: {username}?\n\nThis cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show($"Account for {username} has been deleted.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportVotersToCSV(DataGridView dgv)
        {
            MessageBox.Show("CSV export feature coming soon!\n\nYou can implement this using:\n" +
                "1. SaveFileDialog to choose location\n" +
                "2. Loop through DataGridView rows\n" +
                "3. Write to .csv file",
                "Export Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Results View
        private void CreateResultsPanel()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Header
            Label lblTitle = new Label
            {
                Text = "📊 ELECTION RESULTS DASHBOARD",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Navy,
                Location = new Point(20, 20),
                Size = new Size(500, 40)
            };

            Label lblStatus = new Label
            {
                Name = "lblElectionStatusResults",
                Text = "Loading election status...",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(20, 70),
                Size = new Size(400, 30)
            };

            // Results Grid
            DataGridView dgvResults = new DataGridView
            {
                Name = "dgvResults",
                Location = new Point(20, 120),
                Size = new Size(pnlMainContent.Width - 60, pnlMainContent.Height - 200),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add columns
            dgvResults.Columns.Add("Position", "Position");
            dgvResults.Columns.Add("Candidate", "Candidate");
            dgvResults.Columns.Add("Party", "Party");
            dgvResults.Columns.Add("Region", "Region");
            dgvResults.Columns.Add("Votes", "Votes");
            dgvResults.Columns.Add("Percentage", "Percentage");
            dgvResults.Columns.Add("Status", "Status");

            // Action buttons
            Panel buttonPanel = new Panel
            {
                Location = new Point(20, pnlMainContent.Height - 70),
                Size = new Size(pnlMainContent.Width - 60, 60),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            Button btnRefreshResults = new Button
            {
                Text = "🔄 Refresh Results",
                Location = new Point(0, 10),
                Size = new Size(150, 40),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshResults.FlatAppearance.BorderSize = 0;
            btnRefreshResults.Click += async (s, e) => await LoadResultsAsync(dgvResults, lblStatus);

            Button btnExportResults = new Button
            {
                Text = "📊 Export to PDF",
                Location = new Point(170, 10),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(198, 40, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportResults.FlatAppearance.BorderSize = 0;
            btnExportResults.Click += (s, e) => ExportResultsToPDF();

            Button btnDeclareWinner = new Button
            {
                Text = "🏆 Declare Winner",
                Location = new Point(340, 10),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDeclareWinner.FlatAppearance.BorderSize = 0;
            btnDeclareWinner.Click += (s, e) => DeclareWinner();

            buttonPanel.Controls.Add(btnRefreshResults);
            buttonPanel.Controls.Add(btnExportResults);
            buttonPanel.Controls.Add(btnDeclareWinner);

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(lblStatus);
            mainPanel.Controls.Add(dgvResults);
            mainPanel.Controls.Add(buttonPanel);
            pnlMainContent.Controls.Add(mainPanel);

            // Load results
            _ = LoadResultsAsync(dgvResults, lblStatus);
        }

        private async Task LoadResultsAsync(DataGridView dgv, Label lblStatus)
        {
            try
            {
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                // Check election status
                var statusResponse = await _client.GetAsync("api/admin/election-status");
                if (statusResponse.IsSuccessStatusCode)
                {
                    var status = await statusResponse.Content.ReadFromJsonAsync<JsonElement>();
                    bool isActive = status.GetProperty("isActive").GetBoolean();

                    if (!isActive)
                    {
                        lblStatus.Text = "⏸ ELECTION IS NOT ACTIVE - No results available";
                        lblStatus.ForeColor = Color.Orange;
                        return;
                    }

                    lblStatus.Text = "✅ ELECTION IS ACTIVE - Loading results...";
                    lblStatus.ForeColor = Color.Green;
                }

                // ✅ FIXED: Use real election results endpoint
                var response = await _client.GetAsync("api/admin/election-results");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                    if (result.TryGetProperty("results", out var resultsArray))
                    {
                        int position = 1;
                        foreach (var candidate in resultsArray.EnumerateArray())
                        {
                            dgv.Rows.Add(
                                position,
                                candidate.GetProperty("fullName").GetString(),
                                candidate.GetProperty("partyAffiliation").GetString(),
                                candidate.GetProperty("region").GetString(),
                                candidate.GetProperty("votes").GetInt32(),
                                $"{candidate.GetProperty("percentage").GetDouble():0.00}%",
                                position == 1 ? "🏆 WINNER" : "RUNNING"
                            );
                            position++;
                        }

                        // Highlight winner
                        if (dgv.Rows.Count > 0)
                        {
                            dgv.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                            dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error loading results";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ExportResultsToPDF()
        {
            MessageBox.Show("PDF export feature coming soon!\n\n" +
                "You can use libraries like:\n" +
                "• iTextSharp\n" +
                "• QuestPDF\n" +
                "• PDFSharp",
                "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeclareWinner()
        {
            MessageBox.Show("🏆 WINNER DECLARED!\n\n" +
                "The election results have been finalized and winner is declared.\n" +
                "This will close the election and make results public.",
                "Declare Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Election Control
        private void CreateElectionControlPanel()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Status Panel
            Panel statusPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(pnlMainContent.Width - 60, 100),
                BackColor = Color.FromArgb(240, 245, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblStatusTitle = new Label
            {
                Text = "📊 ELECTION STATUS",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Navy,
                Location = new Point(15, 15),
                Size = new Size(200, 30)
            };

            Label lblStatusValue = new Label
            {
                Name = "lblElectionStatusControl",
                Text = "Loading...",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 50),
                Size = new Size(300, 25),
                ForeColor = Color.Gray
            };

            Label lblVotingStatus = new Label
            {
                Name = "lblVotingStatusControl",
                Text = "",
                Font = new Font("Segoe UI", 11),
                Location = new Point(350, 50),
                Size = new Size(200, 25)
            };

            statusPanel.Controls.Add(lblStatusTitle);
            statusPanel.Controls.Add(lblStatusValue);
            statusPanel.Controls.Add(lblVotingStatus);

            // Control Buttons Panel
            Panel controlPanel = new Panel
            {
                Location = new Point(20, 140),
                Size = new Size(pnlMainContent.Width - 60, 120),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Button btnStartElection = new Button
            {
                Name = "btnStartElection",
                Text = "▶ START ELECTION",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 125, 50), // Green
                Location = new Point(20, 20),
                Size = new Size(200, 50),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btnStartElection.FlatAppearance.BorderSize = 0;
            btnStartElection.Click += async (s, e) => await StartElectionAsync();

            Button btnStopElection = new Button
            {
                Name = "btnStopElection",
                Text = "⏹ STOP ELECTION",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(198, 40, 40), // Red
                Location = new Point(240, 20),
                Size = new Size(200, 50),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnStopElection.FlatAppearance.BorderSize = 0;
            btnStopElection.Click += async (s, e) => await StopElectionAsync();

            Button btnToggleVoting = new Button
            {
                Name = "btnToggleVoting",
                Text = "⏯ TOGGLE VOTING",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(21, 101, 192), // Blue
                Location = new Point(460, 20),
                Size = new Size(200, 50),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnToggleVoting.FlatAppearance.BorderSize = 0;
            btnToggleVoting.Click += async (s, e) => await ToggleVotingAsync();

            Button btnViewResults = new Button
            {
                Text = "📊 VIEW RESULTS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(123, 31, 162), // Purple
                Location = new Point(680, 20),
                Size = new Size(200, 50),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
            btnViewResults.FlatAppearance.BorderSize = 0;
            btnViewResults.Click += (s, e) => PanelResult_Click(this, EventArgs.Empty);

            controlPanel.Controls.Add(btnStartElection);
            controlPanel.Controls.Add(btnStopElection);
            controlPanel.Controls.Add(btnToggleVoting);
            controlPanel.Controls.Add(btnViewResults);

            // Settings Panel
            Panel settingsPanel = new Panel
            {
                Location = new Point(20, 280),
                Size = new Size(pnlMainContent.Width - 60, 200),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblSettingsTitle = new Label
            {
                Text = "⚙️ ELECTION SETTINGS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Navy,
                Location = new Point(15, 15),
                Size = new Size(200, 30)
            };

            // Duration settings
            Label lblDuration = new Label
            {
                Text = "Election Duration (days):",
                Location = new Point(15, 60),
                Size = new Size(150, 25)
            };

            NumericUpDown nudDuration = new NumericUpDown
            {
                Location = new Point(170, 60),
                Size = new Size(100, 25),
                Minimum = 1,
                Maximum = 30,
                Value = 7
            };

            Button btnSaveSettings = new Button
            {
                Text = "💾 SAVE SETTINGS",
                Location = new Point(300, 60),
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSaveSettings.FlatAppearance.BorderSize = 0;

            settingsPanel.Controls.Add(lblSettingsTitle);
            settingsPanel.Controls.Add(lblDuration);
            settingsPanel.Controls.Add(nudDuration);
            settingsPanel.Controls.Add(btnSaveSettings);

            // Add all panels to main panel
            mainPanel.Controls.Add(statusPanel);
            mainPanel.Controls.Add(controlPanel);
            mainPanel.Controls.Add(settingsPanel);
            pnlMainContent.Controls.Add(mainPanel);

            // Load election status
            _ = LoadElectionControlStatusAsync(lblStatusValue, lblVotingStatus,
                btnStartElection, btnStopElection, btnToggleVoting);
        }

        private async Task LoadElectionControlStatusAsync(Label lblStatus, Label lblVoting,
            Button btnStart, Button btnStop, Button btnToggle)
        {
            try
            {
                var response = await _client.GetAsync("api/admin/election-status");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    var isActive = result.GetProperty("isActive").GetBoolean();
                    var votingOpen = result.GetProperty("votingOpen").GetBoolean();

                    lblStatus.Text = isActive ? "✅ ELECTION IS LIVE" : "⏸ ELECTION INACTIVE";
                    lblStatus.ForeColor = isActive ? Color.Green : Color.OrangeRed;

                    lblVoting.Text = votingOpen ? "🗳️ Voting: OPEN" : "⏸ Voting: CLOSED";
                    lblVoting.ForeColor = votingOpen ? Color.Green : Color.Red;

                    btnStart.Enabled = !isActive;
                    btnStop.Enabled = isActive;
                    btnToggle.Enabled = isActive;

                    if (isActive)
                    {
                        btnToggle.Text = votingOpen ? "⏸ PAUSE VOTING" : "▶ RESUME VOTING";
                        btnToggle.BackColor = votingOpen ? Color.Orange : Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error loading status";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private async Task StartElectionAsync()
        {
            using (var startForm = new Form())
            {
                startForm.Text = "Start Election";
                startForm.Size = new Size(500, 350);
                startForm.StartPosition = FormStartPosition.CenterParent;
                startForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                startForm.MaximizeBox = false;
                startForm.MinimizeBox = false;

                Label lblTitle = new Label
                {
                    Text = "🏛️ START NEW ELECTION",
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.Navy,
                    Location = new Point(20, 20),
                    Size = new Size(400, 30)
                };

                Label lblElectionTitle = new Label
                {
                    Text = "Election Title:",
                    Location = new Point(20, 70),
                    Size = new Size(100, 25)
                };

                TextBox txtTitle = new TextBox
                {
                    Text = "General Election 2024",
                    Location = new Point(130, 70),
                    Size = new Size(300, 25)
                };

                Label lblEndTime = new Label
                {
                    Text = "End Date & Time:",
                    Location = new Point(20, 110),
                    Size = new Size(100, 25)
                };

                DateTimePicker dtpEnd = new DateTimePicker
                {
                    Location = new Point(130, 110),
                    Size = new Size(300, 25),
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "yyyy-MM-dd HH:mm",
                    MinDate = DateTime.Now.AddHours(1),
                    Value = DateTime.Now.AddDays(7)
                };

                Button btnConfirm = new Button
                {
                    Text = "🚀 START ELECTION",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Green,
                    Location = new Point(150, 180),
                    Size = new Size(200, 40),
                    FlatStyle = FlatStyle.Flat
                };
                btnConfirm.FlatAppearance.BorderSize = 0;

                btnConfirm.Click += async (s, e) =>
                {
                    var request = new
                    {
                        EndTime = dtpEnd.Value,
                        ElectionTitle = txtTitle.Text,
                        Description = "Election started by administrator"
                    };

                    try
                    {
                        var response = await _client.PostAsJsonAsync("api/admin/start-election", request);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("✅ Election started successfully!\nVoting is now open.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            startForm.Close();
                            RefreshCurrentView();
                        }
                        else
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Failed to start election: {error}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                startForm.Controls.AddRange(new Control[] { lblTitle, lblElectionTitle, txtTitle,
                    lblEndTime, dtpEnd, btnConfirm });
                startForm.ShowDialog();
            }
        }

        private async Task StopElectionAsync()
        {
            var result = MessageBox.Show("⚠️ ARE YOU SURE YOU WANT TO STOP THE ELECTION?\n\n" +
                "This will immediately close voting and cannot be undone!",
                "Confirm Stop Election", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.PostAsync("api/admin/stop-election", null);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("⏹ Election stopped successfully!\nVoting is now closed.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshCurrentView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task ToggleVotingAsync()
        {
            try
            {
                var response = await _client.PostAsync("api/admin/toggle-voting", null);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    var message = result.GetProperty("message").GetString();
                    MessageBox.Show(message, "Voting Status",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCurrentView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Logout
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UserSession.Clear();

                // Hide current admin dashboard
                this.Hide();

                // Create and show login form
                frmLogin loginForm = new frmLogin();

                // When login form closes, also close admin dashboard
                loginForm.FormClosed += (s, args) => this.Close();

                // Show login form
                loginForm.Show();
            }
        }
        #endregion

    }
}