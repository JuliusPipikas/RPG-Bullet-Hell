using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    [Test]
    public void BelowZeroHealthPlayer()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().changeHealth(-12);
        int newHealth = go.GetComponent<PlayerController>().getHealth();

        Assert.AreEqual(newHealth, 0);
    }

    [Test]
    public void BelowZeroHealthEnemy()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyController>();
        go.GetComponent<EnemyController>().changeHealth(-12);
        int newHealth = go.GetComponent<EnemyController>().getHealth();

        Assert.AreEqual(newHealth, 0);
    }

    [Test]
    public void healthBarPlayer()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().addHealthBarForTests();
        GameObject hb = go.GetComponent<PlayerController>().getHealthBar();
        

        go.GetComponent<PlayerController>().changeHealth(-3);

        int index = -Mathf.RoundToInt(hb.GetComponent<SpriteRenderer>().sprite.rect.width);

        Assert.AreEqual(index, 7);
    }

    [Test]
    public void healthBarEnemy()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyController>();
        go.GetComponent<EnemyController>().addHealthBarForTests();
        GameObject hb = go.GetComponent<EnemyController>().getHealthBar();


        go.GetComponent<EnemyController>().setMaxHealth(6);
        go.GetComponent<EnemyController>().changeHealth(-3);

        int index = -Mathf.RoundToInt(hb.GetComponent<SpriteRenderer>().sprite.rect.width);

        Assert.AreEqual(index, 5);
    }

    [Test]
    public void swapPlayerWeaponToStaff()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().SwapWeapon(1);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 0);
    }

    [Test]
    public void swapPlayerWeaponToBook()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().SwapWeapon(2);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 1);
    }

    [Test]
    public void swapPlayerWeaponToCrystal()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().SwapWeapon(3);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 2);
    }

    [Test]
    public void swapPlayerWeaponToSocial()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().SwapWeapon(6);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 3);
    }

    [Test]
    public void swapPlayerWeaponPrevious()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().currentWeapon = 0;
        go.GetComponent<PlayerController>().SwapWeapon(5);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 2);
    }

    [Test]
    public void swapPlayerWeaponNext()
    {
        GameObject go = new GameObject();
        go.AddComponent<PlayerController>();
        go.GetComponent<PlayerController>().currentWeapon = 2;
        go.GetComponent<PlayerController>().SwapWeapon(4);

        Assert.AreEqual(go.GetComponent<PlayerController>().currentWeapon, 0);
    }

    [Test]
    public void PoolerTestActive()
    {
        GameObject go = new GameObject();
        go.AddComponent<Pooler>();
        go.GetComponent<Pooler>().simulateAwake();

        for (int i = 0; i < 3; i++)
        {
            go.GetComponent<Pooler>().GetObject();
        }

        Assert.AreEqual(go.GetComponent<Pooler>().getActiveList().Count, 3);
    }

    [Test]
    public void PoolerTestUsed()
    {
        GameObject go = new GameObject();
        go.AddComponent<Pooler>();
        go.GetComponent<Pooler>().simulateAwake();

        for (int i = 0; i < 3; i++)
        {
            go.GetComponent<Pooler>().GetObject();
        }

        Assert.AreEqual(go.GetComponent<Pooler>().getUsedList().Count, 7);
    }

    [Test]
    public void PoolerTestActiveExpandable()
    {
        GameObject go = new GameObject();
        go.AddComponent<Pooler>();
        go.GetComponent<Pooler>().simulateAwake();
        go.GetComponent<Pooler>().setExpandable();

        for (int i = 0; i < 11; i++)
        {
            go.GetComponent<Pooler>().GetObject();
        }

        Assert.AreEqual(go.GetComponent<Pooler>().getActiveList().Count, 11);
    }

    [Test]
    public void PoolerTestActiveNotExpandable()
    {
        GameObject go = new GameObject();
        go.AddComponent<Pooler>();
        go.GetComponent<Pooler>().simulateAwake();

        for (int i = 0; i < 11; i++)
        {
            go.GetComponent<Pooler>().GetObject();
        }

        Assert.AreEqual(go.GetComponent<Pooler>().getActiveList().Count, 10);
    }

    [Test]
    public void NPCSpeedTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyFactory>();

        go.GetComponent<EnemyFactory>().MockVillagers();
        GameObject vill = go.GetComponent<EnemyFactory>().SpawnVillager("TestVillager", 0, 0, 2);

        Assert.AreEqual(vill.GetComponent<VillagerController>().getMovementVelocity(), 6);
    }

    [Test]
    public void MenuOptionsTest()
    {
        GameObject go = new GameObject();
        MainMenuManager mm = go.AddComponent<MainMenuManager>();
        mm.mockWindows();

        mm.onOptions();

        bool test = false;

        if(!mm.menuActive() && mm.titleActive() && !mm.controlsActive() && mm.optionsActive() && !mm.loadingActive()) { test = true; }

        Assert.IsTrue(test);
    }

    [Test]
    public void MenuControlsTest()
    {
        GameObject go = new GameObject();
        MainMenuManager mm = go.AddComponent<MainMenuManager>();
        mm.mockWindows();

        mm.onControls();

        bool test = false;

        if (!mm.menuActive() && mm.titleActive() && mm.controlsActive() && !mm.optionsActive() && !mm.loadingActive()) { test = true; }

        Assert.IsTrue(test);
    }

    [Test]
    public void MenuLoadingTest()
    {
        GameObject go = new GameObject();
        MainMenuManager mm = go.AddComponent<MainMenuManager>();
        mm.mockWindows();

        mm.onStartGame();

        bool test = false;

        if (!mm.menuActive() && !mm.titleActive() && !mm.controlsActive() && !mm.optionsActive() && mm.loadingActive()) { test = true; }

        Assert.IsTrue(test);
    }

    [Test]
    public void GoblinSpawnTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyFactory>();

        go.GetComponent<EnemyFactory>().MockEnemies();
        GameObject enemy = go.GetComponent<EnemyFactory>().SpawnEnemy("TestGoblin", 0, 0, 0, false, null);

        Assert.AreEqual(enemy.transform.GetChild(0).GetComponent<EnemyController>().GetPooler().name, "TestGoblinProjectiles");
    }

    [Test]
    public void SkeletonSpawnTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyFactory>();

        go.GetComponent<EnemyFactory>().MockEnemies();
        GameObject enemy = go.GetComponent<EnemyFactory>().SpawnEnemy("TestSkeleton", 0, 0, 0, false, null);

        Assert.AreEqual(enemy.transform.GetChild(0).GetComponent<EnemyController>().GetPooler().name, "TestSkeletonProjectiles");
    }

    [Test]
    public void RaiderSpawnTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyFactory>();

        go.GetComponent<EnemyFactory>().MockEnemies();
        GameObject enemy = go.GetComponent<EnemyFactory>().SpawnEnemy("TestRaider", 0, 0, 0, false, null);

        Assert.AreEqual(enemy.transform.GetChild(0).GetComponent<EnemyController>().GetPooler().name, "TestRaiderProjectiles");
    }

    [Test]
    public void WizardSpawnTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<EnemyFactory>();

        go.GetComponent<EnemyFactory>().MockEnemies();
        GameObject enemy = go.GetComponent<EnemyFactory>().SpawnEnemy("TestWizard", 0, 0, 0, false, null);

        Assert.AreEqual(enemy.transform.GetChild(0).GetComponent<EnemyController>().GetPooler().name, "TestWizardProjectiles");
    }
}